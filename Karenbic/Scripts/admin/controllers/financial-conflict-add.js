App.controller('AddFinancialConflictController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, baseUri, toaster, $modal) {

        $scope.customer = null;
        $scope.description = '';
        $scope.items = [];

        $scope.newItem = {
            description: '',
            price: 0
        };

        $scope.showCustomersModal = function () {
            var modalInstance = $modal.open({
                templateUrl: '/CustomersModalContent.html',
                controller: CustomersCtrl,
                size: 'lg'
            });

            modalInstance.result.then(function (result) {
                $scope.customer = result;
            }, function () {
            });
        };

        var CustomersCtrl = ['$scope', '$http', '$modalInstance', function ($scope, $http, $modalInstance) {
            $scope.searchFields = {
                customerName: ''
            };
            $scope.customers = [];

            $scope.fetchCustomers = function (pageIndex) {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'Customer/Get', {
                    params: {
                        customerName: $scope.searchFields.customerName,
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

            $scope.selectCustomer = function (index) {
                $modalInstance.close($scope.customers[index]);
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.addItem = function () {
            $scope.items.push(_.clone($scope.newItem));
            $scope.newItem = {
                description: '',
                price: 0
            };
        };

        $scope.removeItem = function (index) {
            $scope.items.splice(index, 1);
        };

        $scope.add = function () {
            if ($scope.customer == null ||
                $scope.items.length == 0 ||
                addForm.$invalid) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'FinancialConflict/Add',
            {
                model: {
                    Description: $scope.description,
                    Items: _.map($scope.items, function (item) {
                        return {
                            Description: item.description,
                            Price: item.price
                        };
                    })
                },
                customerId: $scope.customer.Id,
                portal: $scope.isDesignPortal() == true ? 1 : 2
            }).
            success(function (data, status, headers, config) {
                $scope.addLoading = false;

                $scope.customer = null;
                $scope.description = '';
                $scope.items = [];
                $scope.newItem = {
                    description: '',
                    price: 0
                };
                $scope.fromSubmited = false;

                toaster.pop('success', "اطلاعات با موفقیت ثبت گردید");
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.addLoading = false;
            });
        };

        $scope.totalPrice = function () {
            var sum = _.reduce($scope.items, function (memo, item) {
                if (!isNaN(item.price))
                    return memo + Number(item.price);
            }, 0);

            return sum;
        };
    }]);