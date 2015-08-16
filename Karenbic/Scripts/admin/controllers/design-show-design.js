App.controller('ShowOrderDesignController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$stateParams', '$upload', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $stateParams, $upload, $state) {
        $scope.orderId = $stateParams.id;
        $scope.order = {};
        $scope.designs = [];
        $scope.states = {
            None: 1,
            Accept: 2,
            Maybe: 3,
            UnAccept: 4
        };

        /*=-=-=-=-=-=-=-=-= Start Order Detail =-=-=-=-=-=-=-=-=*/
        $scope.fetchData = function () {
            $scope.fetchDataLoading = true;

            $http.get(baseUri + 'DesignOrder/SendOrderDesign_GetData', {
                params: {
                    id: $scope.orderId
                }
            })
            .success(function (data, status, headers, config) {
                $scope.order = data;
                $scope.fetchDataLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchDataLoading = false;
            });
        };

        $scope.showDetails = function () {
            var modalInstance = $modal.open({
                templateUrl: '/ShowDetailsContent.html',
                controller: ShowDetailsCtrl,
                size: 'lg',
                resolve: {
                    order: function () {
                        return _.clone($scope.order);
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowDetailsCtrl = ['$scope', '$http', '$modalInstance', 'order', function ($scope, $http, $modalInstance, order) {

            $scope.order = order;

            $scope.fetchValues = function () {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'Order/Details', {
                    params: {
                        orderId: $scope.order.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.values = data;
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
            $scope.fetchValues();

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];
        /*=-=-=-=-=-=-=-=-= End Order Detail =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Designs =-=-=-=-=-=-=-=-=*/
        $scope.fetchDesigns = function () {
            $scope.fetchDesignLoading = true;

            $http.get(baseUri + 'DesignOrder/SendOrderDesign_GetDesigns', {
                params: {
                    id: $scope.orderId
                }
            })
            .success(function (data, status, headers, config) {
                $scope.designs = data.Data;
                $scope.fetchDesignLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchDesignLoading = false;
            });
        };
        /*=-=-=-=-=-=-=-=-= End Designs =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Design Modal =-=-=-=-=-=-=-=-=*/
        $scope.showDesignModal = function (designIndex) {
            var modalInstance = $modal.open({
                templateUrl: '/DesignContent.html',
                controller: ShowDesignCtrl,
                size: 'lg',
                resolve: {
                    design: function () {
                        var design = {
                            Id: $scope.designs[designIndex].Id,
                            Description: $scope.designs[designIndex].Description,
                            PersianRegisterDate: $scope.designs[designIndex].PersianRegisterDate,
                            Time: $scope.designs[designIndex].Time,
                            IsPaidPrepayment: $scope.designs[designIndex].IsPaidPrepayment,
                            IsPaidFinal: $scope.designs[designIndex].IsPaidFinal,
                            Price: $scope.designs[designIndex].Price,
                            Prepayment: $scope.designs[designIndex].Prepayment,
                            Files: []
                        };
                        for (i = 0; i < $scope.designs[designIndex].Files.length; i++) {
                            design.Files.push(_.clone($scope.designs[designIndex].Files[i]));
                        }
                        return design;
                    },
                    order: function () {
                        return {
                            Id: $scope.order.Id,
                            Price: $scope.order.Price,
                            Prepayment: $scope.order.Prepayment,
                            IsPaidPrepayment: $scope.order.IsPaidPrepayment,
                            IsPaidFinal: $scope.order.IsPaidFinal
                        };
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var ShowDesignCtrl = ['$scope', '$http', '$modalInstance', 'design', 'order',function ($scope, $http, $modalInstance, design, order) {

            $scope.design = design;
            $scope.order = order;
            $scope.states = {
                None: 1,
                Accept: 2,
                Maybe: 3,
                UnAccept: 4
            };
            $scope.selectedFile = $scope.design.Files[0];
            $scope.selectedIndex = 0;
            $scope.acceptedFile = null;
            $scope.acceptedIndex = -1;

            $scope.selectFile = function (index) {
                $scope.selectedFile = $scope.design.Files[index];
                $scope.selectedIndex = index;
            };

            $scope.selectNextFile = function () {
                if ($scope.selectedIndex < $scope.design.Files.length - 1) {
                    $scope.selectedIndex++;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = 0;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.selectPrevFile = function () {
                if ($scope.selectedIndex > 0) {
                    $scope.selectedIndex--;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
                else {
                    $scope.selectedIndex = $scope.design.Files.length - 1;
                    $scope.selectedFile = $scope.design.Files[$scope.selectedIndex];
                }
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];
        /*=-=-=-=-=-=-=-=-= End Design Modal =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Send Final Design =-=-=-=-=-=-=-=-=*/
        $scope.finalDesignModal = function () {
            var modalInstance = $modal.open({
                templateUrl: '/SendFinalDesignContent.html',
                controller: FinalDesignCtrl,
                size: 'lg',
                resolve: {
                    orderId: function () {
                        return $scope.order.Id;
                    }
                }
            });

            modalInstance.result.then(function (result) {
            }, function () {
            });
        };

        var FinalDesignCtrl = ['$scope', '$http', '$modalInstance', 'orderId', function ($scope, $http, $modalInstance, orderId) {

            $scope.orderId = orderId;
            $scope.items = [];
            $scope.newItem = {
                Title: '',
                Link: ''
            };

            $scope.fetchFinalDesigns = function () {
                $scope.fetchLoading = true;
 
                $http.get(baseUri + 'DesignOrder_FinalDesign/Get', {
                    params: {
                        orderId: $scope.orderId
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.items = data;
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

            $scope.addItem = function () {
                $scope.items.push(_.clone($scope.newItem));
                $scope.newItem = {
                    Title: '',
                    Link: ''
                };
            };

            $scope.removeItem = function (index) {
                $scope.items.splice(index, 1);
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

            $scope.save = function () {
                $scope.fromSubmited = true;
                if ($scope.form.$invalid || $scope.items.length == 0) return;

                $scope.addLoading = true;
                $http.post(baseUri + 'DesignOrder_FinalDesign/Save',
                {
                    orderId: $scope.orderId,
                    designs: _.map($scope.items, function (item) {
                        return {
                            Title: item.Title,
                            Link: item.Link
                        };
                    })
                }).
                success(function (data, status, headers, config) {
                    $scope.addLoading = false;

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

            //init
            $scope.fetchFinalDesigns();
        }];
        /*=-=-=-=-=-=-=-=-= End Send Final Design =-=-=-=-=-=-=-=-=*/

        $scope.back = function (index) {
            $scope.fetchLoading = true;
            $state.go('^.finished-order-list');
        };

        //init
        $scope.fetchData();
        $scope.fetchDesigns();
    }]);