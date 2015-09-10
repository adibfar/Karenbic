App.controller('AddProductController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.newProduct = {
            category: null,
            title: '',
            pictureFile: null,
            renderedPictureFile: null,
            priority: 0,
            price: 0
        };
        $scope.categories = [];
        $scope.pictures = [];

        $scope.fetchCategories = function () {
            $scope.fetchCategoryLoading = true;

            $http.get(baseUri + 'ProductCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                if ($scope.categories.length > 0)
                    $scope.newProduct.category = $scope.categories[0];
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
                $scope.newProduct.pictureFile = $file;
                $scope.newProduct.renderedPictureFile = e.target.result;
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.onPictureSelect = function ($files) {
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
                $scope.pictures.push({
                    file: $file,
                    renderedFile: e.target.result
                });
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.removePicture = function (index) {
            $scope.pictures.splice(index, 1);
        };

        $scope.add = function () {
            if ($scope.newProduct.category == null ||
                $scope.newProduct.title.trim() == '' ||
                isNaN($scope.newProduct.priority) == true ||
                isNaN($scope.newProduct.price) == true ||
                $scope.newProduct.pictureFile == null ||
                $scope.pictures.length == 0) return;

            $scope.addLoading = true;

            var files = [];
            files.push($scope.newProduct.pictureFile);
            _.each($scope.pictures, function (item) {
                files.push(item.file);
            });

            var filesIndex = -1;
            var filesName = [];
            filesName.push("mainFile");
            _.each($scope.pictures, function (item) {
                filesIndex++;
                filesName.push('pictures[' + filesIndex + ']');
            });

            $upload.upload({
                url: baseUri + 'Product/Add',
                file: files,
                fileFormDataName: filesName,
                data: {
                    categoryId: $scope.newProduct.category.Id,
                    title: $scope.newProduct.title,
                    priority: $scope.newProduct.priority,
                    price: $scope.newProduct.price,
                    description: $scope.newProduct.description
                }
            }).success(function (data, status, headers, config) {
                $scope.newProduct = {
                    title: '',
                    pictureFile: null,
                    renderedPictureFile: null,
                    priority: 0,
                    price: 0
                };
                $scope.newProduct.category = $scope.categories[0];
                $scope.pictures = [];
                toaster.pop('success', "محصول جدید با موفقیت ثبت گردید");
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

        //init
        $scope.fetchCategories();
    }]);