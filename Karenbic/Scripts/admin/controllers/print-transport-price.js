App.controller('TransportPriceController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {

        $scope.transportPrice = {};

        $scope.fetchTransportPrice = function () {
            $scope.fetchLoading = true;
            $http.get(baseUri + 'TransportPrice/Get')
            .success(function (data, status, headers, config) {
                $scope.transportPrice = data;
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
            $http.post(baseUri + 'TransportPrice/Edit',
            {
                bikeDelivery: $scope.transportPrice.BikeDelivery,
                tipax: $scope.transportPrice.Tipax,
                porterage: $scope.transportPrice.Porterage
            }).
            success(function (data, status, headers, config) {
                $scope.sendLoading = false;
                toaster.pop('success', "اطلاعات با موفقیت ذخیره گردید");
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
        $scope.fetchTransportPrice();

    }]);