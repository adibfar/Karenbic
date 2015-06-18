/**=========================================================
 * Module: design-payment-checkout.js
 * Show Paid Factor
 =========================================================*/

App.controller('DesignPaymentCheckoutController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$stateParams',
    function ($scope, $http, baseUri, toaster, $stateParams) {
        $scope.paymentId = $stateParams.id;

        $scope.fetchData = function () {
            if ($scope.paymentId == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'DesignPayment/GetCheckoutData', {
                params: {
                    paymentId: $scope.paymentId
                }
            })
            .success(function (data, status, headers, config) {
                if (data.Data.payment.IsPaid) {
                    $scope.payment = data.Data.payment;
                    $scope.factors = data.Data.factors;
                }
                else {
                    toaster.pop('error', "مبلغ مورد نظر پرداخت نگردید");
                }
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
        $scope.fetchData();

        $scope.totalPrice = function () {
            if ($scope.factors == null || $scope.factors == undefined || $scope.factors.length == 0) return 0;

            var sum = _.reduce($scope.factors, function (memo, item) { return memo + item.Price; }, 0);

            return sum;
        };
    }]);