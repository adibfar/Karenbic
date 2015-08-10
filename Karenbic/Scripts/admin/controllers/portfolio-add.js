App.controller('AddPortfolioController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.types = [];
        $scope.categories = [];

        $scope.fetchTypes = function () {
            $scope.fetchTypesLoading = true;

            $http.get(baseUri + 'PortfolioType/Get')
            .success(function (data, status, headers, config) {
                $scope.types = data.Data;
                $scope.newPortfolio.Type = $scope.types[0];
                $scope.fetchTypesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchTypesLoading = false;
            });
        };

        $scope.$watch(function () {
            return $scope.newPortfolio.Type;
        }, function (newValue, oldValue) {
            $scope.fetchCategories();
        });

        $scope.fetchCategories = function () {
            if ($scope.newPortfolio.Type == null) return;

            $scope.fetchCategoriesLoading = true;

            $http.get(baseUri + 'PortfolioCategory/Get', {
                params: {
                    typeId: $scope.newPortfolio.Type.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.categories = data.Data;
                $scope.newPortfolio.Category = $scope.categories[0];
                $scope.fetchCategoriesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCategoriesLoading = false;
            });
        };

        /*=-=-=-=-=-=-=-= Start Add =-=-=-=-=-=-=-=*/
        $scope.newPortfolio = {
            Type: null,
            Category: null,
            Priority: 0,
            Description: '',
            TumbPictureFile: null,
            PictureFile: null,
            ReadedPictureFile: null
        };

        $scope.onPictureSelect = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 500 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                return;
            }

            $scope.newPortfolio.PictureFile = $file;

            var reader = new FileReader();
            reader.onload = function (e) {
                $scope.newPortfolio.ReadedPictureFile = e.target.result;
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.onTumbPictureSelect = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 500 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                return;
            }

            $scope.newPortfolio.TumbPictureFile = $file;

            var reader = new FileReader();
            reader.onload = function (e) {
                $('#tumbPic').attr('src', e.target.result);
            }
            reader.readAsDataURL($file);
        };

        $scope.addFormIsValide = function () {
            if ($scope.newPortfolio.Type == null ||
                $scope.newPortfolio.Category == null ||
                isNaN($scope.newPortfolio.Priority) ||
                $scope.newPortfolio.TumbPictureFile == null ||
                $scope.newPortfolio.PictureFile == null)
                return false;
            return true;
        };

        $scope.add = function () {
            if ($scope.addFormIsValide() == false) return;

            $scope.addLoading = true;

            $upload.upload({
                url: baseUri + 'Portfolio/Add',
                file: [$scope.newPortfolio.TumbPictureFile, $scope.newPortfolio.PictureFile],
                fileFormDataName: ["tumbPicture", "picture"],
                data: {
                    categoryId: $scope.newPortfolio.Category.Id,
                    priority: $scope.newPortfolio.Priority,
                    description: $scope.newPortfolio.Description
                }
            }).success(function (data, status, headers, config) {
                $scope.newPortfolio = {
                    Type: null,
                    Category: null,
                    Priority: 0,
                    Description: '',
                    TumbPictureFile: null,
                    PictureFile: null,
                    ReadedPictureFile: null
                };
                $('.NFI-wrapper').find('input[type=file]').val('');
                $('.NFI-wrapper').find('input[type=text]').val('');
                
                toaster.pop('success', "اطلاعات با موفقیت ثبت گردید");

                $scope.addLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.addLoading = false;
            });
        };
        /*=-=-=-=-=-=-=-= End Add =-=-=-=-=-=-=-=*/
        //init
        $scope.fetchTypes();
    }]);