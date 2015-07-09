/**=========================================================
 * Module: print-new-order-list.js
 * Show Factor List & Payment That
 =========================================================*/

App.controller('FactorListOfDesignController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$state',
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

            $http.get(baseUri + 'FactorOfDesignOrder/Get', {
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

            var checkItems = _.find($scope.factors, function (item) {
                return item.PrepaymentFactor.IsPaid == false &&
                    item.PrepaymentFactor.IsChecked != true &&
                    item.FinalFactor.IsChecked == true;
            });

            if (checkItems != undefined && checkItems != null) return false;

            var prepaymentFilters = _.filter($scope.factors, function (item) {
                return item.PrepaymentFactor.IsChecked != null &&
                    item.PrepaymentFactor.IsChecked != undefined &&
                    item.PrepaymentFactor.IsChecked == true;
            });

            var finalFilters = _.filter($scope.factors, function (item) {
                return item.FinalFactor.IsChecked != null &&
                    item.FinalFactor.IsChecked != undefined &&
                    item.FinalFactor.IsChecked == true;
            });

            if ((prepaymentFilters != null && prepaymentFilters.length > 0) ||
                (finalFilters != null && finalFilters.length > 0)) return true;
            return false;
        };

        $scope.paymentPreview = function (index) {
            if ($scope.factors[index].PrepaymentFactor.IsChecked != true &&
                $scope.factors[index].FinalFactor.IsChecked != true) {
                toaster.pop('warning', "هیچیک از مبالغ انتخاب نشده");
            }
            else if ($scope.factors[index].PrepaymentFactor.IsPaid == false &&
                $scope.factors[index].PrepaymentFactor.IsChecked != true &&
                $scope.factors[index].FinalFactor.IsChecked == true) {
                toaster.pop('warning', "لطفاً ابتدا مبلغ پیش پرداخت را واریز نمایید");
            }
            else {
                $scope.fetchLoading = true;

                var id = [];

                if ($scope.factors[index].PrepaymentFactor.IsChecked == true)
                    id.push("p" + $scope.factors[index].PrepaymentFactor.Id);

                if ($scope.factors[index].FinalFactor.IsChecked == true)
                    id.push("f" + $scope.factors[index].FinalFactor.Id);

                $state.go('^.payment-preview', { id: id });
            }
        };

        $scope.paymentPreview_group = function () {
            $scope.fetchLoading = true;

            var checkItems = _.find($scope.factors, function (item) {
                return item.PrepaymentFactor.IsPaid == false &&
                    item.PrepaymentFactor.IsChecked != true &&
                    item.FinalFactor.IsChecked == true;
            });

            if (checkItems != undefined && checkItems != null) {
                toaster.pop('warning', "لطفاً ابتدا مبلغ پیش پرداخت را واریز نمایید");
                return;
            }

            var prepaymentFilters = _.filter($scope.factors, function (item) {
                return item.PrepaymentFactor.IsChecked != null &&
                    item.PrepaymentFactor.IsChecked != undefined &&
                    item.PrepaymentFactor.IsChecked == true;
            });

            var prepaymentsId = _.map(prepaymentFilters, function (item) {
                return 'p' + item.Id;
            });

            var finalFilters = _.filter($scope.factors, function (item) {
                return item.FinalFactor.IsChecked != null &&
                    item.FinalFactor.IsChecked != undefined &&
                    item.FinalFactor.IsChecked == true;
            });

            var finalsId = _.map(finalFilters, function (item) {
                return 'f' + item.Id;
            });

            var id = prepaymentsId.concat(finalsId);

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