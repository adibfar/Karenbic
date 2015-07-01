App.controller('SendOrderDesignController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$stateParams', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $stateParams, $upload) {
        $scope.orderId = $stateParams.id;
        $scope.order = {};
        $scope.designs = [];
        $scope.newDesign = {
            description: '',
            files: []
        };
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

        var ShowDetailsCtrl = function ($scope, $http, $modalInstance, order) {

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
        };
        /*=-=-=-=-=-=-=-=-= End Order Detail =-=-=-=-=-=-=-=-=*/

        /*=-=-=-=-=-=-=-=-= Start Send Design =-=-=-=-=-=-=-=-=*/
        $scope.openFileDialog = function () {
            $("#newFile").click();
        };

        $scope.onFileSelect = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 250 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                return;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                $scope.newDesign.files.push({
                    file: $file,
                    renderedFile: e.target.result
                });
                $scope.$apply();
            }
            reader.readAsDataURL($file);
        };

        $scope.removeFileOfNewDesign = function (index) {
            $scope.newDesign.files.splice(index, 1);
        }

        $scope.add = function () {
            if ($scope.newDesign.files.length == 0) return;

            $scope.addLoading = true;

            var files = _.map($scope.newDesign.files, function (item) {
                return item.file;
            });

            var index = -1;
            var fileFormDataName = _.map($scope.newDesign.files, function (item) {
                index++;
                return 'file[' + index + ']';
            });

            $upload.upload({
                url: baseUri + 'DesignOrder/SendOrderDesign',
                file: files,
                fileFormDataName: fileFormDataName,
                data: {
                    orderId: $scope.orderId,
                    description: $scope.newDesign.description
                }
            }).success(function (data, status, headers, config) {
                $scope.newDesign = {
                    description: '',
                    files: []
                };
                $scope.designs.unshift(data);
                toaster.pop('success', "اطلاعات با موفقیت ارسال گردید");
                $scope.addLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.addLoading = false;
            });
        }
        /*=-=-=-=-=-=-=-=-= End Send Design =-=-=-=-=-=-=-=-=*/

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

        /*=-=-=-=-=-=-=-=-= Start Design =-=-=-=-=-=-=-=-=*/
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

        var ShowDesignCtrl = function ($scope, $http, $modalInstance, design, order) {

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
        };
        /*=-=-=-=-=-=-=-=-= End Design =-=-=-=-=-=-=-=-=*/

        //init
        $scope.fetchData();
        $scope.fetchDesigns();
    }]);