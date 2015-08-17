App.controller('FinancialConflictPaymentPreviewController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$stateParams',
    function ($scope, $http, ngDialog, baseUri, toaster, $stateParams) {
        $scope.id = $stateParams.id;

        $scope.fetchData = function () {
            if ($scope.id == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'FinancialConflict/PaymentPreview_Data', {
                params: {
                    id: $scope.id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.factor = data.factor;
                $scope.now = data.now;
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

        $scope.fetchBankGetway = function () {
            if ($scope.id == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'FinancialConflict/PaymentPreview_GetGeteway_FAKE', {
                params: {
                    id: $scope.id
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