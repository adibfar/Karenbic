/**=========================================================
 * Module: print-error-payment.js
 * Handle Payment Error
 =========================================================*/

App.controller('ErrorPaymentController', ['$scope', '$stateParams',
    function ($scope, $stateParams) {

        $scope.errorCode = $stateParams.code;

        if ($scope.errorCode != null || $scope.errorCode != undefined) {
            switch ($scope.errorCode) {
                case "11":
                    $scope.errorText = "شماره کارت نا معتبر است";
                    break;
                case "12":
                    $scope.errorText = "موجودی کافی نیست";
                    break;
                case "13":
                    $scope.errorText = "شماره کارت نا معتبر است";
                    break;
                case "14":
                    $scope.errorText = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است";
                    break;
                case "15":
                    $scope.errorText = "كارت نامعتبر است";
                    break;
                case "16":
                    $scope.errorText = "دفعات برداشت وجه بيش از حد مجاز است";
                    break;
                case "17":
                    $scope.errorText = "كاربر از انجام تراكنش منصرف شده است";
                    break;
                case "18":
                    $scope.errorText = "تاريخ انقضاي كارت گذشته است";
                    break;
                case "19":
                    $scope.errorText = "مبلغ برداشت وجه بيش از حد مجاز است";
                    break;
                case "111":
                    $scope.errorText = "صادر كننده كارت نامعتبر است";
                    break;
                case "112":
                    $scope.errorText = "خطاي سوييچ صادر كننده كارت";
                    break;
                case "113":
                    $scope.errorText = "پاسخي از صادر كننده كارت دريافت نشد";
                    break;
                case "114":
                    $scope.errorText = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                //custome error
                case "9999":
                    $scope.errorText = "خطا در اطلاعات ورودی";
                    break;
            }
        }

    }]);