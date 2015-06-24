App.controller('PriceListController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.priceLists = [];

        $scope.fetchPriceLists = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'PriceList/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2
                }
            })
            .success(function (data, status, headers, config) {
                $scope.priceLists = data.Data;
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
        $scope.newPriceList = {
            Title: '',
            Order: 0,
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

            $scope.newPriceList.PictureFile = $file;

            var reader = new FileReader();
            reader.onload = function (e) {
                $('#pricePic').attr('src', e.target.result);
            }
            reader.readAsDataURL($file);
        };

        $scope.add = function () {
            if ($scope.addPriceListForm.$invalid) return;

            $scope.addLoading = true;

            $upload.upload({
                url: baseUri + 'PriceList/Add',
                file: $scope.newPriceList.PictureFile,
                data: {
                    title: $scope.newPriceList.Title,
                    order: $scope.newPriceList.Order,
                    portal: $scope.isDesignPortal() == true ? 1 : 2
                }
            }).success(function (data, status, headers, config) {
                $scope.newPriceList = {
                    Title: '',
                    Order: 0,
                    PictureFile: null
                };
                $('#pricePic').attr('src', '');
                $('#pricePic').val('');
                $('#pricePic').closest('.form-group').find('input[type=text]').val('');
                $scope.priceLists.push(data);
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
                    $http.post(baseUri + 'PriceList/Remove',
                    {
                        id: $scope.priceLists[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.priceLists.splice(index, 1);
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
                    priceList: function () {
                        return _.clone($scope.priceLists[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.priceLists[index].Title = result.Title;
                $scope.priceLists[index].Order = result.Order;
                $scope.priceLists[index].PictureFile = result.PictureFile;
                $scope.priceLists[index].PicturePath = result.PicturePath;
            }, function () {
            });
        };

        var EditCtrl = function ($scope, $http, $modalInstance, priceList) {

            $scope.priceList = priceList;
            $scope.newPictureFile = null;

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
                $http.post(baseUri + 'PriceList/Edit',
                {
                    id: $scope.priceList.Id,
                    title: $scope.priceList.Title,
                    order: $scope.priceList.Order
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
                    url: baseUri + 'PriceList/Edit',
                    file: $scope.newPictureFile,
                    data: {
                        id: $scope.priceList.Id,
                        title: $scope.priceList.Title,
                        order: $scope.priceList.Order
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
        };

        $scope.showPreviewModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/PreviewModalContent.html',
                controller: EditCtrl,
                size: 'xs',
                resolve: {
                    priceList: function () {
                        return _.clone($scope.priceLists[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        //init
        $scope.fetchPriceLists();
    }]);