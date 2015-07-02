App.controller('PreDesignOrderController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 
    function ($scope, $http, baseUri, toaster, $modal) {

        $scope.text = '';

        $scope.fetchText = function () {
            $scope.fetchLoading = true;
            $http.get(baseUri + 'DesignOrder/PreOrderText')
            .success(function (data, status, headers, config) {
                $scope.text = data;
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

        //init 
        $scope.fetchText();
    }]);