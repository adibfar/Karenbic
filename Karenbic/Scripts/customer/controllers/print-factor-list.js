/**=========================================================
 * Module: print-new-order-list.js
 * Show Factor List & Payment That
 =========================================================*/

App.controller('FactorListOfPrintController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $state) {
        $scope.searchFields = {
            isPaid: true,
            isNotPaid: true,
            startDate: '',
            endDate: ''
        };
        $scope.factors = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.fetchFactors = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'FactorOfPrintOrder/Get', {
                params: {
                    isPaid: $scope.searchFields.isPaid,
                    isNotPaid: $scope.searchFields.isNotPaid,
                    startDate: $scope.searchFields.startDate,
                    endDate: $scope.searchFields.endDate,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.factors = data.Data.List;
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
            $scope.fetchFactors(1);
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
            $scope.fetchFactors($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchFactors($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchFactors($scope.pageIndex - 1);
            }
        };

        $scope.anyFactorSelected = function () {
            if ($scope.factors == null || $scope.factors == undefined || $scope.factors.length == 0) return false;

            var items = _.filter($scope.factors, function (item) { return item.IsChecked == true; });

            if (items != null && items.length > 0) return true;
            return false;
        };

        $scope.paymentPreview = function (index) {
            $scope.fetchLoading = true;
            $state.go('^.payment-preview', { id: $scope.factors[index].Factor.Id });
        };

        $scope.paymentPreview_group = function () {
            $scope.fetchLoading = true;

            var filters = _.filter($scope.factors, function (item) {
                return item.IsChecked != null && item.IsChecked != undefined && item.IsChecked == true;
            });

            var id = _.map(filters, function (item) {
                return item.Factor.Id;
            });
            
            $state.go('^.payment-preview', { id: id });
        };

        $scope.showDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowDetailsContent.html',
                controller: ShowDetailsCtrl,
                size: 'lg',
                resolve: {
                    factor: function () {
                        return _.clone($scope.factors[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowDetailsCtrl = ['$scope', '$http', '$modalInstance', 'factor', function ($scope, $http, $modalInstance, factor) {

            $scope.factor = factor;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'FactorOfOrder/Details', {
                    params: {
                        orderId: $scope.factor.Id
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

        $scope.showPaymentDetails = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ShowPaymentDetailsContent.html',
                controller: ShowPaymentDetailsCtrl,
                size: 'lg',
                resolve: {
                    factor: function () {
                        return _.clone($scope.factors[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowPaymentDetailsCtrl = ['$scope', '$http', '$modalInstance', 'factor', function ($scope, $http, $modalInstance, factor) {

            $scope.factor = factor;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'PrintPayment/Details', {
                    params: {
                        id: $scope.factor.Payment.Id
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
        }];

        //Init
        $scope.fetchFactors(1);
    }]);