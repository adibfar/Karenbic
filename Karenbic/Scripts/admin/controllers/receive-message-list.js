App.controller('ReceiveMessageListController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog', '$sce',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog, $sce) {
        $scope.messages = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;
        $scope.resultCount = 0;

        $scope.fetchMessages = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'ReceiveMessage/Get', {
                params: {
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.messages = data.Data.List;
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

        $scope.generatePagation = function () {
            $scope.pages = [];
            if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
            if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
            $scope.pages.push($scope.pageIndex);
            if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
            if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
        };

        $scope.changePage = function (index) {
            $scope.fetchMessages($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchMessages($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchMessages($scope.pageIndex - 1);
            }
        };

        $scope.showMessage = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/MessageModalContent.html',
                controller: MessageCtrl,
                size: 'lg',
                resolve: {
                    message: function () {
                        return _.clone($scope.messages[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.messages[index].IsRead = true;
                $scope.messages[index].IsAdminReply = true;
                $scope.messages[index].AdminReply = result.AdminReply;
            }, function () {
                $scope.messages[index].IsRead = true;
            });
        };

        var MessageCtrl = ['$scope', '$http', '$modalInstance', 'message', function ($scope, $http, $modalInstance, message) {

            $scope.message = message;
            $scope.message.Text2 = $sce.trustAsHtml($scope.message.Text);
            $scope.message.AdminReply2 = $sce.trustAsHtml($scope.message.AdminReply);

            $scope.froalaOptions = {
                buttons: ["bold", "italic", "underline", "strikeThrough", "fontFamily", "fontSize", "color",
                    "sep",
                    "formatBlock", "align", "insertOrderedList", "insertUnorderedList", "outdent", "indent", "selectAll",
                    "sep",
                    "insertHorizontalRule", "createLink", "table", "insertImage", "insertVideo", "undo", "redo", "html"],
                inlineMode: false,
                inverseSkin: true,
                allowedImageTypes: ["jpeg", "jpg", "png"],
                height: 150,
                language: "fa",
                direction: "rtl",
                fontList: ["Tahoma, Geneva", "Arial, Helvetica", "Impact, Charcoal"],
                spellcheck: true,
                plainPaste: true,
                borderColor: '#00008b',
                enableScript: false
            };

            $scope.markAsRead = function () {
                $http.post(baseUri + 'ReceiveMessage/MarkAsRead',
                {
                    id: $scope.message.Id,
                }).
                success(function (data, status, headers, config) {
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                });
            };
            if ($scope.message.IsRead == false) {
                $scope.markAsRead();
            }

            $scope.reply = function () {
                if ($scope.replyForm.$invalid) return;

                $scope.replyLoading = true;
                $http.post(baseUri + 'ReceiveMessage/Reply',
                {
                    id: $scope.message.Id,
                    text: $scope.message.AdminReply
                }).
                success(function (data, status, headers, config) {
                    $scope.replyLoading = false;
                    $modalInstance.close($scope.message);
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    $scope.replyLoading = false;
                });
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

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
                    $http.post(baseUri + 'ReceiveMessage/Remove',
                    {
                        id: $scope.messages[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchMessages($scope.pageIndex);
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
        $scope.fetchMessages(1);
    }]);