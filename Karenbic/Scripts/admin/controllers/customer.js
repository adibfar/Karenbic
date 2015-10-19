App.controller('CustomerController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal) {

        $scope.searchFields = {
            customerName: '',
            customerGroup: null,
            province: null,
            city: null
        };
        $scope.customerGroups = [];
        $scope.provinces = [];
        $scope.cities = [];
        $scope.customers = [];

        $scope.fetchCustomerGroups = function () {
            $scope.fetchCustomerGroupsLoading = true;
            $http.get(baseUri + 'CustomerGroup/Get')
            .success(function (data, status, headers, config) {
                $scope.customerGroups = data.Data;
                $scope.customerGroups.unshift({
                    Id: 0,
                    Title: "انتخاب گروه"
                });
                $scope.searchFields.customerGroup = $scope.customerGroups[0];
                $scope.fetchCustomerGroupsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCustomerGroupsLoading = false;
            });
        };

        $scope.fetchProvinces = function () {
            $scope.fetchProvincesLoading = true;
            $http.get(baseUri + 'Province/Get')
            .success(function (data, status, headers, config) {
                $scope.provinces = data.Data;
                $scope.provinces.unshift({
                    Id: 0,
                    Name: "انتخاب استان"
                });
                $scope.searchFields.province = $scope.provinces[0];
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
            if ($scope.searchFields.province == null) return;

            $scope.fetchCitiesLoading = true;
            $http.get(baseUri + 'City/Get', {
                params: {
                    provinceId: $scope.searchFields.province.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.cities = data.Data;
                $scope.cities.unshift({
                    Id: 0,
                    Name: "انتخاب شهر"
                });
                $scope.searchFields.city = $scope.cities[0];
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

        $scope.fetchCustomers = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'Customer/Get', {
                params: {
                    customerName: $scope.searchFields.customerName,
                    customerGroupId: $scope.searchFields.customerGroup != null && $scope.searchFields.customerGroup.Id != 0 ? $scope.searchFields.customerGroup.Id : null,
                    provinceId: $scope.searchFields.province != null && $scope.searchFields.province.Id != 0 ? $scope.searchFields.province.Id : null,
                    cityId: $scope.searchFields.city != null && $scope.searchFields.city.Id != 0 ? $scope.searchFields.city.Id : null,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.customers = data.Data.List;
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
            $scope.fetchCustomers(1);
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
            $scope.fetchCustomers($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchCustomers($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchCustomers($scope.pageIndex - 1);
            }
        };

        $scope.showEditModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/EditModalContent.html',
                controller: EditCtrl,
                size: 'lg',
                resolve: {
                    customerGroups: function () {
                        return _.clone($scope.customerGroups);
                    },
                    provinces: function () {
                        return _.clone($scope.provinces);
                    },
                    customer: function () {
                        return _.clone($scope.customers[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.fetchCustomers($scope.pageIndex);
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'customerGroups', 'provinces', 'customer', function ($scope, $http, $modalInstance, customerGroups, provinces, customer) {

            $scope.customerGroups = customerGroups;
            $scope.provinces = provinces;
            $scope.cities = [];
            $scope.customer = customer;
            $scope.firstTimeFetchCities = true;

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
                $http.post(baseUri + 'Customer/Edit',
                {
                    customer: {
                        Id: $scope.customer.Id,
                        Name: $scope.customer.Name,
                        Surname: $scope.customer.Surname,
                        Gender: $scope.customer.Gender,
                        Phone: $scope.customer.Phone,
                        Mobile: $scope.customer.Mobile,
                        Email: $scope.customer.Email,
                        Address: $scope.customer.Address
                    },
                    cityId: $scope.customer.City.Id,
                    customerGroupId: $scope.customer.Group != null && $scope.customer.Group.Id != 0 ? $scope.customer.Group.Id : null
                }).
                success(function (data, status, headers, config) {
                    $scope.editLoading = false;
                    $modalInstance.close(data);
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

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            //_______ init
            //select province
            $scope.provinces.shift();
            var provinceIndex = _.findIndex($scope.provinces, function (item) {
                return item.Id == $scope.customer.Province.Id;
            });
            $scope.customer.Province = $scope.provinces[provinceIndex];

            //select cities
            $scope.fetchCities();

            //select group
            var customerGroupIndex = _.findIndex($scope.customerGroups, function (item) {
                return item.Id == $scope.customer.Group.Id;
            });
            $scope.customer.Group = $scope.customerGroups[customerGroupIndex];
            
        }];

        $scope.showChangePassModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/ChangePassModalContent.html',
                controller: ChangePassCtrl,
                size: 'sm',
                resolve: {
                    customer: function () {
                        return _.clone($scope.customers[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ChangePassCtrl = ['$scope', '$http', '$modalInstance', 'customer', function ($scope, $http, $modalInstance, customer) {
            $scope.customer = customer;

            $scope.edit = function () {
                if ($scope.editCustomerForm.$invalid) return;

                $scope.editLoading = true;
                $http.post(baseUri + 'Customer/ChangePassword',
                {
                    customerId: $scope.customer.Id,
                    newPassword: $scope.newPass
                }).
                success(function (data, status, headers, config) {
                    $scope.editLoading = false;
                    $modalInstance.close(data);
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

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.disableCustomer = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'Customer/DisableUser',
            {
                customerId: $scope.customers[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.customers[index].IsActive = false;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.fetchLoading = false;
            });
        };

        $scope.enableCustomer = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'Customer/EnableUser',
            {
                customerId: $scope.customers[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.customers[index].IsActive = true;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.fetchLoading = false;
            });
        };

        //init
        $scope.fetchCustomerGroups();
        $scope.fetchProvinces();
        $scope.fetchCustomers(1);
    }]);