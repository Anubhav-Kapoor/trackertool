﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Task_and_Leave_Tracker.SignIn" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="import" href="header.html">
    <title>Task & Leave Tracker Tool-Sign in Page</title>
</head>
<body ng-app="myApp" ng-controller="loginCtrl" style="background-image :url(images/background_pattern.jpg);">
    <div class="container container-resize" style="background-color: #f5f5f5;">
        <div header></div>

        <div class="row" style="margin-top:20px">
            <div class="col-xs-12 col-sm-8 col-md-6 col-sm-offset-2 col-md-offset-3">
                <form role="form" id="login_form" method="post">
                    <fieldset>
                        <h2>Please Sign In</h2>
                        <hr class="colorgraph">

                        <div class="form-group">
                            <input type="text" name="ntid_name" id="login_ntid" class="form-control input-lg" ng-model="ntid" placeholder="Enter NTID...">
                        </div>
                        <div class="form-group">
                            <input type="password" name="password" id="password" class="form-control input-lg"  ng-model="password" placeholder="Password...">
                        </div>
                        <span class="checkbox">
                            <%--<button type="button" class="btn" data-color="info">Remember Me</button>--%>


                            <%--<div class="checkbox">--%>
                                <label>
                                    <input type="checkbox" checked data-toggle="toggle" data-style="slow">
                                    Remember Me
                                </label>
                            <%--</div>--%>



                            <input type="checkbox" name="remember_me" id="remember_me" checked="checked" class="hidden">
                            <a class="forgot btn btn-link pull-right" ng-click="forgotPwdPopUp()">Forgot Password?</a>

                        </span>
                        <hr class="colorgraph">
                        <div class="row">
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <input type="button" class="button"  ng-click="login()" value="Sign In">
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <a href="/SignUp.aspx" class="btn btn-default button" style="padding-top:16px">Register</a>
                            </div>
                        </div>
                    </fieldset>
                </form>
            </div>
        </div>
       <%-- <div id="myPopup" style="display:none;">
            <fieldset>
                <h2>Forgot Password</h2>
                <hr class="colorgraph">

                <div class="row">
                    <div class="col-8">


                        <div class="form-group">
                            <label class="col-md-4 control-label">NTID</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <input  name="ntid_name" placeholder="Enter your NTID" class="form-control" style="border-radius:10px" type="text">
                                </div>
                            </div>
                            <button class="button">Submit</button>
                        </div>


                        <!--<label>Enter your NTID</label>
                        <input type="text" />
                        -->

                    </div>
                </div>
            </fieldset>
        </div>--%>
    </div>
     <div class="modal fade" id="forgotPwd" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
            
            <h4 class="modal-title" id="myModalLabel">Send Password</h4>
            </div>
              <form id="forgot_form" method="post">
            <div class="modal-body">

              
               <div class="form-group" style="margin-bottom:34px">
                            <%--<label class="col-md-4 control-label"  >E-Mail</label>--%>
                            <div class="col-md-4 inputGroupContainer" style="width:70%">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-screenshot"></i></span>
                                    <input id="forgot_ntid" name="ntid_name" placeholder="Network ID"  class="form-control" type="text"  ng-model="ntid"/>
                                </div>
                            </div>
                        </div>


            </div>
            <div class="modal-footer">
                 <a href="#" class="btn btn-default button" style="padding-top:16px" data-dismiss="modal" ng-click="sendPwd()">Send</a>
                <a href="#" class="btn btn-default button" style="padding-top:16px" data-dismiss="modal">Close</a>
                
        </div>
            </form>
    </div>
  </div>
</div>
</body>
</html>

