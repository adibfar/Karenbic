﻿App.controller('FinancialConflictListController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.searchFields = {
            customerName: '',
            states: null,
            startDate: '',
            endDate: ''
        };
        $scope.items = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;
        $scope.totalPrice = 0;

        $scope.fetchData = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'FinancialConflict/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2,
                    customerName: $scope.searchFields.customerName,
                    startDate: $scope.searchFields.startDate,
                    endDate: $scope.searchFields.endDate,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.items = data.Data.List;
                $scope.pageCount = data.Data.PageCount;
                $scope.pageIndex = data.Data.PageIndex;
                $scope.resultCount = data.Data.ResultCount;
                $scope.totalPrice = data.Data.TotalPrice;
                $scope.generatePagation();
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

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.fetchData(1);
        }

        $scope.generatePagation = function () {
            $scope.pages = [];
            if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
            if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
            $scope.pages.push($scope.pageIndex);
            if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
            if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
        };

        $scope.changePage = function (index) {
            $scope.fetchData($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchData($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchData($scope.pageIndex - 1);
            }
        };

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
                    $http.post(baseUri + 'FinancialConflict/Remove',
                    {
                        id: $scope.items[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchData($scope.pageIndex);
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
                templateUrl: '/EditContent.html',
                controller: EditCtrl,
                size: 'lg',
                resolve: {
                    item: function () {
                        return _.clone($scope.items[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.orders[index].Price = result;
                $scope.orders[index].IsConfirm = true;
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'item', function ($scope, $http, $modalInstance, item) {

            $scope.customer = null;
            $scope.description = '';
            $scope.items = [];

            $scope.newItem = {
                Description: '',
                Price: 0
            };

            $scope.fetchItem = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'FinancialConflict/Find', {
                    params: {
                        id: item.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.customer = data.Customer;
                    $scope.description = data.Description;
                    $scope.items = data.Items;

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

            $scope.totalPrice = function () {
                var sum = _.reduce($scope.items, function (memo, item) {
                    if (!isNaN(item.Price))
                        return memo + Number(item.Price);
                }, 0);

                return sum;
            };

            $scope.save = function () {
                $scope.confirmOrderFromSubmited = true;
                if ($scope.confirmOrderFrom.$invalid) return;

                $scope.confirmLoading = true;
                $http.post(baseUri + 'PrintOrder/Confirm',
                {
                    orderId: $scope.order.Id,
                    price: $scope.order.Price
                }).
                success(function (data, status, headers, config) {
                    $scope.confirmLoading = false;
                    if (data == "True") {
                        $modalInstance.close($scope.order.Price);
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    $scope.confirmLoading = false;
                });
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            //init
            $scope.fetchItem();
        }];

        //Init
        $scope.fetchData(1);
    }]);