/**=========================================================
 * Module: design-canceled-order-list.js
 * Show Caneled Order List
 =========================================================*/

App.controller('CanceledDesignOrderListController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal) {
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

            $http.get(baseUri + 'DesignOrder/GetCanceledOrders', {
                params: {
                    orderId: $scope.searchFields.orderId.trim() != '' ? Number($scope.searchFields.orderId) : null,
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

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        //Init
        $scope.fetchOrders(1);
    }]);