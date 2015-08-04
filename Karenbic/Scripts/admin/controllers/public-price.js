App.controller('PublicPriceController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.categories = [];
        $scope.selectedCategory = null;
        $scope.prices = [];

        $scope.fetchCategories = function () {
            $scope.fetchCategoriesLoading = true;
            $http.get(baseUri + 'PublicPriceCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                $scope.newPrice.Category = $scope.categories[0];
                $scope.selectedCategory = $scope.categories[0];
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

        $scope.$watch(function () {
            return $scope.selectedCategory;
        }, function (newValue, oldValue) {
            if(newValue != null)
            {
                $scope.fetchPrices();
            }
        });

        $scope.fetchPrices = function () {
            if ($scope.selectedCategory == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PublicPrice/Get', {
                params: {
                    categoryId: $scope.selectedCategory.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.prices = data.Data;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchLoading = false;
            });
        };

        /*=-=-=-=-=-=-=-= Start Add =-=-=-=-=-=-=-=*/
        $scope.newPrice = {
            Category: null,
            Title: '',
            Priority: 0,
            PictureFile: null
        };

        $scope.onFileSelect = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 250 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                return;
            }

            $scope.newPrice.PictureFile = $file;

            var reader = new FileReader();
            reader.onload = function (e) {
                $('#pricePic').attr('src', e.target.result);
            }
            reader.readAsDataURL($file);
        };

        $scope.add = function () {
            if ($scope.addPriceForm.$invalid &&
                $scope.newPrice.Category == null) return;

            $scope.addLoading = true;

            $upload.upload({
                url: baseUri + 'PublicPrice/Add',
                file: $scope.newPrice.PictureFile,
                data: {
                    title: $scope.newPrice.Title,
                    priority: $scope.newPrice.Priority,
                    categoryId: $scope.newPrice.Category.Id
                }
            }).success(function (data, status, headers, config) {
                $scope.newPrice = {
                    Category: $scope.categories[0],
                    Title: '',
                    Priority: 0,
                    PictureFile: null
                };
                $('#pricePic').attr('src', '');
                $('#pricePic').val('');
                $('#pricePic').closest('.form-group').find('input[type=text]').val('');
                if ($scope.selectedCategory.Id == $scope.newPrice.Category.Id)
                    $scope.fetchPrices();
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

        $scope.remove = function (index) {
            ngDialog.open({
                template: 'removeDialog.html',
                showClose: false,
                controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                    $scope.close = function () {
                        $scope.closeThisDialog(0);
                    };
                    $scope.confirm = function () {
                        $scope.closeThisDialog(1);
                    };
                }],
                preCloseCallback: function (value) {
                    if (value != 1) return true;

                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'PublicPrice/Remove',
                    {
                        id: $scope.prices[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.prices.splice(index, 1);
                        }
                        else {
                            toaster.pop('error', "امکان حذف اطلاعات وجود ندارد");
                        }
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.fetchLoading = false;
                    });
                    return true;
                }
            });
        };

        $scope.showEditModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/EditModalContent.html',
                controller: EditCtrl,
                size: 'xs',
                resolve: {
                    categories: function(){
                        return _.clone($scope.categories);
                    },
                    priceList: function () {
                        var output = {
                            Id: $scope.prices[index].Id,
                            Title: $scope.prices[index].Title,
                            Priority: $scope.prices[index].Priority,
                            PictureFile: $scope.prices[index].PictureFile,
                            PicturePath: $scope.prices[index].PicturePath,
                            Category: {
                                Id: $scope.prices[index].Category.Id,
                                Title: $scope.prices[index].Category.Title
                            }
                        };
                        return _.clone($scope.prices[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.fetchPrices();
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'categories', 'priceList', function ($scope, $http, $modalInstance, categories, priceList) {

            $scope.categories = categories;
            $scope.priceList = priceList;
            $scope.newPictureFile = null;

            var categoryIndex = _.findIndex($scope.categories, function (item) {
                return item.Id == $scope.priceList.Category.Id;
            });
            $scope.priceList.Category = $scope.categories[categoryIndex];

            $scope.onFileSelect = function ($files) {
                var $file = $files[0];

                if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                    return;
                }

                if ($file.size > 250 * 1024) {
                    toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                    return;
                }

                $scope.newPictureFile = $file;

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#editPricePic').attr('src', e.target.result);
                }
                reader.readAsDataURL($file);
            };

            $scope.edit = function () {
                if ($scope.editPriceListForm.$invalid) return;
                $scope.editLoading = true;
                if ($scope.newPictureFile == null) $scope.edit_1();
                else $scope.edit_2();
            };

            $scope.edit_1 = function () {
                $http.post(baseUri + 'PublicPrice/Edit',
                {
                    id: $scope.priceList.Id,
                    title: $scope.priceList.Title,
                    priority: $scope.priceList.Priority,
                    categoryId: $scope.priceList.Category.Id
                }).
                success(function (data, status, headers, config) {
                    $scope.editLoading = false;
                    $modalInstance.close(data);
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

            $scope.edit_2 = function () {
                $upload.upload({
                    url: baseUri + 'PublicPrice/Edit',
                    file: $scope.newPictureFile,
                    data: {
                        id: $scope.priceList.Id,
                        title: $scope.priceList.Title,
                        priority: $scope.priceList.Priority,
                        categoryId: $scope.priceList.Category.Id
                    }
                }).success(function (data, status, headers, config) {
                    $scope.addLoading = false;
                    $modalInstance.close(data);
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

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.showPreviewModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/PreviewModalContent.html',
                controller: EditCtrl,
                size: 'xs',
                resolve: {
                    categories: function () {
                        return _.clone($scope.categories);
                    },
                    priceList: function () {
                        return _.clone($scope.prices[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        //init
        $scope.fetchCategories();
    }]);