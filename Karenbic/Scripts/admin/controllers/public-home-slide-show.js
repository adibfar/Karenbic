App.controller('HomeSlideShowController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.slides = [];

        $scope.fetchSlides = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'HomeSlideShow/Get')
            .success(function (data, status, headers, config) {
                $scope.slides = data;
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

        /*=-=-=-=-=-=-=-= Start Add =-=-=-=-=-=-=-=*/
        $scope.newSlide = {
            Priority: 0,
            PictureFile: null
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

            $scope.newSlide.PictureFile = $file;

            var reader = new FileReader();
            reader.onload = function (e) {
                $('#pricePic').attr('src', e.target.result);
            }
            reader.readAsDataURL($file);
        };

        $scope.add = function () {
            if ($scope.addSlideForm.$invalid && $scope.newSlide.PictureFile != null) return;

            $scope.addLoading = true;

            $upload.upload({
                url: baseUri + 'HomeSlideShow/Add',
                file: $scope.newSlide.PictureFile,
                data: {
                    priority: $scope.newSlide.Priority
                }
            }).success(function (data, status, headers, config) {
                $scope.newSlide = {
                    Priority: 0,
                    PictureFile: null
                };
                $('#pricePic').attr('src', '');
                $('#pricePic').val('');
                $('#pricePic').closest('.form-group').find('input[type=text]').val('');
                $scope.slides.push(data);
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
        };
        /*=-=-=-=-=-=-=-= End Add =-=-=-=-=-=-=-=*/

        $scope.remove = function (index) {
            ngDialog.open({
                template: 'removeDialog.html',
                showClose: false,
                controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                    $scope.close = function () {
                        $scope.closeThisDialog(0);
                    };
                    $scope.confirm = function () {
                        $scope.closeThisDialog(1);
                    };
                }],
                preCloseCallback: function (value) {
                    if (value != 1) return true;

                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'HomeSlideShow/Remove',
                    {
                        id: $scope.slides[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.slides.splice(index, 1);
                        }
                        else {
                            toaster.pop('error', "امکان حذف اطلاعات وجود ندارد");
                        }
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.fetchLoading = false;
                    });
                    return true;
                }
            });
        };

        //init
        $scope.fetchSlides();
    }]);