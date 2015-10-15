App.controller('ChangeMobileNumberController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {

        $scope.number = '';

        $scope.fetchNumber = function () {
            $scope.fetchLoading = true;
            $http.get(baseUri + 'Profile/GetMobileNumber')
            .success(function (data, status, headers, config) {
                $scope.number = data;
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

        $scope.save = function () {
            $scope.sendLoading = true;
            $http.post(baseUri + 'Profile/ChangeMobileNumber',
            {
                number: $scope.number
            }).
            success(function (data, status, headers, config) {
                $scope.sendLoading = false;
                toaster.pop('success', "شماره موبایل با موفقیت ذخیره گردید");
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.sendLoading = false;
            });
        };

        //init 
        $scope.fetchNumber();

    }]);