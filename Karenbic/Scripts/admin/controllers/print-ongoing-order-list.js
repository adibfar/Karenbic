/**=========================================================
 * Module: print-ongoing-order-list.js
 * Show Ongoing Order List
 =========================================================*/

App.controller('OngoingPrintOrderListController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal) {
        $scope.searchFields = {
            orderId: '',
            states: null,
            startDate: '',
            endDate: ''
        };
        $scope.orders = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.orderStates = [
            {
                Text: 'در انتظار چاپ',
                Value: 2
            },
            {
                Text: 'در انتظار خدمات پس از چاپ',
                Value: 3
            },
            {
                Text: 'سفارش آماده تحویل',
                Value: 4
            },
            {
                Text: 'سفارش تحویل داده شد',
                Value: 5
            }
        ];

        $scope.searchOrderStates = [
            {
                Text: 'در انتظار چاپ',
                Value: 2
            },
            {
                Text: 'در انتظار خدمات پس از چاپ',
                Value: 3
            },
            {
                Text: 'سفارش آماده تحویل',
                Value: 4
            }
        ];

        $scope.fetchOrders = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'PrintOrder/GetOngoingOrders', {
                params: {
                    orderId: $scope.searchFields.orderId.trim() != '' ? Number($scope.searchFields.orderId) : null,
                    startDate: $scope.searchFields.startDate,
                    endDate: $scope.searchFields.endDate,
                    states: _.map($scope.searchFields.states, function (item) {
                        return item.Value;
                    }),
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.orders = data.Data.List;
                $scope.pageCount = data.Data.PageCount;
                $scope.pageIndex = data.Data.PageIndex;
                $scope.resultCount = data.Data.ResultCount;
                $scope.setOrderSate();
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
            $scope.fetchOrders(1);
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
            $scope.fetchOrders($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchOrders($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchOrders($scope.pageIndex - 1);
            }
        };

        //Order State
        $scope.setOrderSate = function () {
            _.each($scope.orders, function (item) {
                switch (item.OrderState) {
                    case 2:
                        item.State = $scope.orderStates[0];
                        break;

                    case 3:
                        item.State = $scope.orderStates[1];
                        break;

                    case 4:
                        item.State = $scope.orderStates[2];
                        break;

                    case 5:
                        item.State = $scope.orderStates[3];
                        break;
                }
            });
        };

        $scope.changeOrderState = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'PrintOrder/ChangeState',
            {
                orderId: $scope.orders[index].Id,
                state: $scope.orders[index].State.Value
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True" && $scope.orders[index].State.Value == 5) {
                    $scope.fetchOrders($scope.pageIndex);
                }
                else if (data == "False") {
                    toaster.pop('error', "امکان تغییر وضعیت سفارش وجود ندارد");
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
        };

        $scope.anyOrdersSelected = function () {
            if ($scope.orders == null || $scope.orders == undefined || $scope.orders.length == 0) return false;

            var items = _.filter($scope.orders, function (item) { return item.IsChecked == true; });

            if (items != null && items.length > 0) return true;
            return false;
        };

        $scope.changeOrdersState = function () {
            $scope.fetchLoading = true;

            var filters = _.filter($scope.orders, function (item) {
                return item.IsChecked != null && item.IsChecked != undefined && item.IsChecked == true;
            });

            var id = _.map(filters, function (item) {
                return item.Id;
            });

            $http.post(baseUri + 'PrintOrder/ChangeOrdersState',
            {
                ordersId: id,
                state: $scope.selectedOrdersState.Value
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True") {
                    $scope.fetchOrders($scope.pageIndex);
                }
                else {
                    toaster.pop('error', "امکان تغییر وضعیت سفارش وجود ندارد");
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
        };

        $scope.cancel = function (index) {
            ngDialog.open({
                template: 'cancelOrderDialog.html',
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
                    $http.post(baseUri + 'Order/CancelOrder',
                    {
                        orderId: $scope.orders[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchOrders($scope.pageIndex);
                        }
                        else {
                            toaster.pop('error', "امکان کنسل کردن سفارش وجود ندارد");
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

        $scope.showDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowDetailsContent.html',
                controller: ShowDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.orders[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.orders[index].Price = result;
                $scope.orders[index].IsConfirm = true;
            }, function () {
            });
        };

        var ShowDetailsCtrl = function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'Order/Details', {
                    params: {
                        orderId: $scope.order.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.values = data;
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
            $scope.fetchValues();

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
        };

        $scope.showPaymentDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowPaymentDetailsContent.html',
                controller: ShowPaymentDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.orders[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowPaymentDetailsCtrl = function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'PrintPayment/Details', {
                    params: {
                        id: $scope.order.Payment.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.payment = data.Data.payment;
                    $scope.factors = data.Data.factors;
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
            $scope.fetchValues();

            $scope.totalPrice = function () {
                if ($scope.factors == null || $scope.factors == undefined || $scope.factors.length == 0) return 0;

                var sum = _.reduce($scope.factors, function (memo, item) { return memo + item.Price; }, 0);

                return sum;
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        };

        //Init
        $scope.fetchOrders(1);
    }]);