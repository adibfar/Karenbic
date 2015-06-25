App.controller('ChangePasswordController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {

        $scope.edit = function () {
            if ($scope.editPasswordForm.$invalid) return;

            $scope.editLoading = true;
            $http.post(baseUri + 'Profile/ChangePassword',
            {
                newPassword: $scope.newPass,
                reNewPassword: $scope.reNewPass
            }).
            success(function (data, status, headers, config) {
                $scope.editLoading = false;
                toaster.pop('success', "رمز عبور تغییر یافت");
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.editLoading = false;
            });
        };

    }]);