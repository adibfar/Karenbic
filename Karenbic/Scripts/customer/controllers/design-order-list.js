/**=========================================================
 * Module: design-order-list.js
 * Show Order List
 =========================================================*/

App.controller('DesignOrderListController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $state) {
        $scope.searchFields = {
            orderId: '',
            startDate: '',
            endDate: ''
        };
        $scope.orders = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.fetchOrders = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'DesignOrder/Get', {
                params: {
                    orderId: $scope.searchFields.orderId,
                    startDate: $scope.searchFields.startDate,
                    endDate: $scope.searchFields.endDate,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.orders = data.Data.List;
                $scope.pageCount = data.Data.PageCount;
                $scope.pageIndex = data.Data.PageIndex;
                $scope.resultCount = data.Data.ResultCount;
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
            }, function () {
            });
        };

        var ShowDetailsCtrl = ['$scope', '$http', '$modalInstance', 'order', function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'FactorOfOrder/Details', {
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

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.showPaymentDetails = function (paymentId) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowPaymentDetailsContent.html',
                controller: ShowPaymentDetailsCtrl,
                size: 'lg',
                resolve: {
                    paymentId: function () {
                        return paymentId;
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowPaymentDetailsCtrl = ['$scope', '$http', '$modalInstance', 'paymentId', function ($scope, $http, $modalInstance, paymentId) {

            $scope.paymentId = paymentId;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'DesignPayment/Details', {
                    params: {
                        id: $scope.paymentId
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
                if ($scope.orders == null || $scope.orders == undefined || $scope.orders.length == 0) return 0;

                var sum = _.reduce($scope.orders, function (memo, item) { return memo + item.Price; }, 0);

                return sum;
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.show = function (index) {
            $scope.fetchLoading = true;
            $state.go('^.show-order', { id: $scope.orders[index].Id });
        };

        $scope.print = function (index) {
            $scope.fetchLoading = true;
            $state.go('app.print.add-order');
        };

        /*=-=-=-=-=-=-=-=-= Start Final Design =-=-=-=-=-=-=-=-=*/

        $scope.showFinalDesignModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/FinalDesignContent.html',
                controller: FinalShowDesignCtrl,
                size: 'md',
                resolve: {
                    orderId: function () {
                        return $scope.orders[index].Id;
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var FinalShowDesignCtrl = ['$scope', '$http', '$modalInstance', 'orderId', function ($scope, $http, $modalInstance, orderId) {

            $scope.orderId = orderId;
            $scope.finalDesigns = [];

            $scope.fetchFinalDesigns = function () {
                $scope.fetchFinalDesignsLoading = true;

                $http.get(baseUri + 'DesignOrder_FinalDesign/Get', {
                    params: {
                        orderId: $scope.orderId
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.finalDesigns = data;
                    $scope.fetchFinalDesignsLoading = false;
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                    }
                    $scope.fetchFinalDesignsLoading = false;
                });
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            //init
            $scope.fetchFinalDesigns();
        }];
        /*=-=-=-=-=-=-=-=-= End Final Design =-=-=-=-=-=-=-=-=*/

        //Init
        $scope.fetchOrders(1);
    }]);