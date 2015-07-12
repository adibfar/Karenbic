App.controller('ChangeProfileController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {
        $scope.customer = {};
        $scope.provinces = [];
        $scope.cities = [];
        $scope.firstTimeFetchProvinces = true;
        $scope.firstTimeFetchCities = true;

        $scope.fetchProfile = function () {
            $scope.fetchLoading = true;
            $http.get(baseUri + 'Profile/GetProfile')
            .success(function (data, status, headers, config) {
                $scope.customer = data;
                $scope.fetchProvinces();
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

        $scope.fetchProvinces = function () {
            $scope.fetchProvincesLoading = true;
            $http.get(baseUri + 'Province/Get')
            .success(function (data, status, headers, config) {
                $scope.provinces = data.Data;
                if ($scope.firstTimeFetchProvinces) {
                    $scope.firstTimeFetchProvinces = false;
                    var provinceIndex = _.findIndex($scope.provinces, function (item) {
                        return item.Id == $scope.customer.Province.Id;
                    });
                    $scope.customer.Province = $scope.provinces[provinceIndex];
                    $scope.fetchCities();
                }
                $scope.fetchProvincesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchProvincesLoading = false;
            });
        };

        $scope.fetchCities = function () {
            if ($scope.customer.Province == null) return;

            $scope.fetchCitiesLoading = true;
            $http.get(baseUri + 'City/Get', {
                params: {
                    provinceId: $scope.customer.Province.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.cities = data.Data;
                if ($scope.firstTimeFetchCities) {
                    $scope.firstTimeFetchCities = false;
                    var cityIndex = _.findIndex($scope.cities, function (item) {
                        return item.Id == $scope.customer.City.Id;
                    });
                    $scope.customer.City = $scope.cities[cityIndex];
                }
                else {
                    $scope.customer.City = $scope.cities[0];
                }
                $scope.fetchCitiesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCitiesLoading = false;
            });
        };

        $scope.edit = function () {
            if ($scope.editCustomerForm.$invalid) return;

            $scope.editLoading = true;
            $http.post(baseUri + 'Profile/Edit',
            {
                customer: {
                    Name: $scope.customer.Name,
                    Surname: $scope.customer.Surname,
                    Gender: $scope.customer.Gender,
                    Phone: $scope.customer.Phone,
                    Mobile: $scope.customer.Mobile,
                    Email: $scope.customer.Email,
                    Address: $scope.customer.Address
                },
                cityId: $scope.customer.City.Id,
            }).
            success(function (data, status, headers, config) {
                $scope.editLoading = false;
                toaster.pop('success', "اطلاعات با موفقیت تغییر یافت");
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

        //init
        $scope.fetchProfile();
    }]);