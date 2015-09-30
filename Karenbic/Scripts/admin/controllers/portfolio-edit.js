App.controller('EditPortfolioController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload', '$stateParams',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload, $stateParams) {
        $scope.portfolio = {};
        $scope.types = [];
        $scope.categories = [];
        $scope.newMainPicture = null;
        $scope.newMainRenderedPicture = null;
        $scope.removedPictures = [];
        $scope.newPictures = [];

        $scope.fetchPortfolio = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'Portfolio/Find', {
                params: {
                    id: $stateParams.id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.portfolio = data;
                $scope.fetchTypes();
                //$scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    if ($("#portal").val() == 1) {
                        toaster.pop('error', "Error occurred. Refresh Page");
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                    }
                }
                $scope.fetchLoading = false;
            });
        };

        $scope.fetchTypes = function () {
            $scope.fetchTypesLoading = true;

            $http.get(baseUri + 'PortfolioType/Get')
            .success(function (data, status, headers, config) {
                $scope.types = data.Data;
                var index = _.findIndex($scope.types, function (item) {
                    return item.Id == $scope.portfolio.Type.Id;
                });
                $scope.portfolio.Type = $scope.types[index];
                $scope.fetchTypesLoading = false;
                $scope.fetchCategories();
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

        $scope.fetchCategories = function () {
            $scope.fetchCategoryLoading = true;

            $http.get(baseUri + 'PortfolioCategory/Get', {
                params: {
                    typeId: $scope.portfolio.Type.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.categories = data.Data;
                var index = _.findIndex($scope.categories, function (item) {
                    return item.Id == $scope.portfolio.Category.Id;
                });
                $scope.portfolio.Category = $scope.categories[index];
                $scope.fetchCategoryLoading = false;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCategoryLoading = false;
            });
        };

        $scope.$watch(function () {
            return $scope.portfolio.Type;
        }, function (newValue, oldValue) {
            if ($scope.portfolio.Type == undefined ||
                $scope.portfolio.Type == 'undefined' ||
                $scope.portfolio.Type == null)
                return;

            $scope.fetchCategoryLoading = true;

            $http.get(baseUri + 'PortfolioCategory/Get', {
                params: {
                    typeId: $scope.portfolio.Type.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.categories = data.Data;
                $scope.portfolio.Category = $scope.categories[0];
                $scope.fetchCategoryLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCategoryLoading = false;
            });
        });

        $scope.onMainPictureSelect = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                if ($("#portal").val() == 1) {
                    toaster.pop('error', "File needs to have an extension type of .png or .jpg");
                }
                else {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                }
                return;
            }

            if ($file.size > 150 * 1024) {
                if ($("#portal").val() == 1) {
                    toaster.pop('error', "The file you are trying to send exceeds the 150KB attachment limit");
                }
                else {
                    toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                }
                return;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                $scope.newMainPicture = $file;
                $scope.newMainRenderedPicture = e.target.result;
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.addNewPicture = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                if ($("#portal").val() == 1) {
                    toaster.pop('error', "File needs to have an extension type of .png or .jpg");
                }
                else {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                }
                return;
            }

            if ($file.size > 150 * 1024) {
                if ($("#portal").val() == 1) {
                    toaster.pop('error', "The file you are trying to send exceeds the 150KB attachment limit");
                }
                else {
                    toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                }
                return;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                $scope.newPictures.push({
                    file: $file,
                    renderedFile: e.target.result
                });
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.removeOldPicture = function (index) {
            $scope.removedPictures.push($scope.portfolio.Pictures[index].Id);
            $scope.portfolio.Pictures.splice(index, 1);
        };

        $scope.removeNewPicture = function (index) {
            $scope.newPictures.splice(index, 1);
        };

        $scope.edit = function () {
            if ($scope.portfolio.Category == null ||
                $scope.portfolio.Title.trim() == '' ||
                isNaN($scope.portfolio.Priority) == true) return;

            $scope.editLoading = true;

            var files = [];
            if ($scope.newMainPicture != null) {
                files.push($scope.newMainPicture);
            }
            _.each($scope.newPictures, function (item) {
                files.push(item.file);
            });

            var filesIndex = -1;
            var filesName = [];
            if ($scope.newMainPicture != null) {
                filesName.push("mainFile");
            }
            _.each($scope.newPictures, function (item) {
                filesIndex++;
                filesName.push('newPictures[' + filesIndex + ']');
            });

            $upload.upload({
                url: baseUri + 'Portfolio/Edit',
                file: files,
                fileFormDataName: filesName,
                data: {
                    id: $scope.portfolio.Id,
                    categoryId: $scope.portfolio.Category.Id,
                    title: $scope.portfolio.Title,
                    priority: $scope.portfolio.Priority,
                    description: $scope.portfolio.Description,
                    removedPictures: $scope.removedPictures.join(',')
                }
            }).success(function (data, status, headers, config) {
                toaster.pop('success', "محصول با موفقیت ویرایش گردید");
                $scope.newMainPicture = null;
                $scope.newMainRenderedPicture = null;
                $scope.removedPictures = [];
                $scope.newPictures = [];
                $scope.fetchPortfolio();
                $scope.editLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.editLoading = false;
            });
        };

        //init
        $scope.fetchPortfolio();
    }]);