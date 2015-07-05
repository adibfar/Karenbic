﻿/**=========================================================
 * Module: print-payment-preview.js
 * Show Payment Preview
 =========================================================*/

App.controller('PrintPaymentPreviewController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$stateParams',
    function ($scope, $http, ngDialog, baseUri, toaster, $stateParams) {
        $scope.factorIds = $stateParams.id.split(',');

        $scope.fetchData = function () {
            if ($scope.factorIds == null && $scope.factorIds.length == 0) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PrintPayment/GetPreviewData', {
                params: {
                    factorsId: _.map($scope.factorIds, function (item) {
                        return Number(item);
                    })
                }
            })
            .success(function (data, status, headers, config) {
                $scope.factors = data.Data.factors;
                $scope.now = data.Data.now;
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

        $scope.fetchBankGetway = function () {
            if ($scope.factorIds == null && $scope.factorIds.length == 0) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PrintPayment/GetGeteway_FAKE', {
                    params: {
                        factorsId: _.map($scope.factorIds, function (item) {
                            return Number(item);
                        })
                    }
                })
            .success(function (data, status, headers, config) {
                if (data.ResCode == "0") {
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "https://bpm.shaparak.ir/pgwchannel/startpay.mellat");
                    form.setAttribute("target", "_self");
                    var hiddenField = document.createElement("input");
                    hiddenField.setAttribute("name", "RefId");
                    hiddenField.setAttribute("value", data.RefId);
                    form.appendChild(hiddenField);
                    document.body.appendChild(form);
                    form.submit();
                    document.body.removeChild(form);
                }
                else {
                    $scope.fetchLoading = false;
                    toaster.pop('error', "خطایی رخ داده، مجدداً تلاش کنید");
                }
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده، مجدداً تلاش کنید");
                }
                $scope.fetchLoading = false;
            });
        }
    }]);