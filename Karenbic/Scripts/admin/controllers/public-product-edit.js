App.controller('EditProductController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload', '$stateParams',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload, $stateParams) {
        $scope.product = { };
        $scope.categories = [];
        $scope.newMainPicture = null;
        $scope.newMainRenderedPicture = null;
        $scope.removedPictures = [];
        $scope.newPictures = [];

        $scope.fetchProduct = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'Product/Find', {
                params: {
                    id: $stateParams.id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.product = data;
                $scope.fetchCategories();
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

        $scope.fetchCategories = function () {
            $scope.fetchCategoryLoading = true;

            $http.get(baseUri + 'ProductCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                var index = _.findIndex($scope.categories, function (item) {
                    return item.Id == $scope.product.Category.Id;
                });
                $scope.product.Category = $scope.categories[index];
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
            $scope.removedPictures.push($scope.product.Pictures[index].Id);
            $scope.product.Pictures.splice(index, 1);
        };

        $scope.removeNewPicture = function (index) {
            $scope.newPictures.splice(index, 1);
        };

        $scope.edit = function () {
            if ($scope.product.Category == null ||
                $scope.product.Title.trim() == '' ||
                isNaN($scope.product.Priority) == true ||
                isNaN($scope.product.Price) == true) return;

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
                url: baseUri + 'Product/Edit',
                file: files,
                fileFormDataName: filesName,
                data: {
                    id: $scope.product.Id,
                    categoryId: $scope.product.Category.Id,
                    title: $scope.product.Title,
                    priority: $scope.product.Priority,
                    price: $scope.product.Price,
                    description: $scope.product.Description,
                    removedPictures: $scope.removedPictures.join(',')
                }
            }).success(function (data, status, headers, config) {
                toaster.pop('success', "محصول با موفقیت ویرایش گردید");
                $scope.newMainPicture = null;
                $scope.newMainRenderedPicture = null;
                $scope.removedPictures = [];
                $scope.newPictures = [];
                $scope.fetchProduct();
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
        $scope.fetchProduct();
    }]);