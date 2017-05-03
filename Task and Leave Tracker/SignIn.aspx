<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Task_and_Leave_Tracker.SignIn" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="import" href="header.html" />
    <title>Task & Leave Tracker Tool-Sign in Page</title>
</head>
<body ng-app="myApp" ng-controller="loginCtrl" ng-init="pageInit()" style="background-image: url(images/background_pattern.jpg);">
    <div class="container container-resize" style="background-color: #f5f5f5;">
        <div header></div>

        <div class="row" style="margin-top: 20px">
            <div class="col-xs-12 col-sm-8 col-md-6 col-sm-offset-2 col-md-offset-3">
                <form role="form" id="login_form" method="post">
                    <fieldset>
                        <h2>Please Sign In</h2>
                        <hr class="colorgraph" />

                        <div class="form-group">
                            <input type="text" name="ntid_name" id="login_ntid" class="form-control input-lg" ng-model="ntid" placeholder="Enter NTID..." />
                        </div>
                        <div class="form-group">
                            <input type="password" name="password" id="password" class="form-control input-lg" ng-model="password" placeholder="Password..." />
                        </div>
                        <span class="checkbox">

                            <label>
                                <input type="checkbox" checked data-toggle="toggle" data-style="slow" />
                                Remember Me
                            </label>

                            <input type="checkbox" name="remember_me" id="remember_me" checked="checked" class="hidden" />
                            <a class="forgot btn btn-link pull-right" ng-click="forgotPwdPopUp()">Forgot Password?</a>

                        </span>

                        <hr class="colorgraph" />
                        <div class="row form-group">
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <button class="button" ng-click="login()" value="Sign In">Sign In</button>

                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <a href="/SignUp.aspx" class="btn btn-default button" style="padding-top: 16px">Register</a>
                            </div>
                        </div>
                    </fieldset>
                </form>
            </div>
        </div>

    </div>
    <!--Forgot Password Modal-->
    <div class="modal fade" id="forgotPwd" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" id="myModalLabel" style="text-align: center">Forgot Password</h4>
                </div>
                <form id="forgot_form" class="form-horizontal">
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-md-4 control-label">NTID</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-screenshot"></i></span>
                                    <input id="forgot_ntid" name="ntid_name" placeholder="Network ID" class="form-control" type="text" ng-model="forgot_ntid" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="button" ng-click="sendPwd()">Send</button>
                        <button class="button" data-dismiss="modal">Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- Sign-In Authentication Status Modal -->
    <div class="modal fade" id="statusModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalTitle" style="text-align: center">Authentication <span ng-bind="status"></span></h4>
                </div>
                <div class="modal-body">
                    <h3 style="text-align: center" ng-bind="reason"></h3>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Loader -->
    <div style="position: fixed; top: 0; left: 0; right: 0; bottom: 0; margin: auto; z-index: 3500 ; display: none" class="loader la-ball-fussion la-3x">
        <div style="color: #d8edee;"></div>
        <div style="color: #ff414d;"></div>
        <div style="color: #1aa6b7;"></div>
        <div style="color: #022c42;"></div>
    </div>
    <div class="backdrop" style="z-index: 2500; display: none">
    </div>

</body>
</html>

