﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Task_and_Leave_Tracker.SignUp" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head  >
    <link rel="import" href="header.html" />
    <title>Sign Up Team Member Page</title>
</head>
<body ng-app="myApp" ng-controller="loginCtrl" ng-init="pageInit()" style="background-image: url(images/background_pattern.jpg);">
  <form class="well form-horizontal" id="register_form">
        <div class="container" style="background-color: #f5f5f5;">
            <div header=""></div>
            <div class="row">
                <div class="col-2">

                    <!--Empty Section-->
                    <fieldset>

                        <!-- Form Name -->
                        <legend style="padding-left: 15px;">Create your Account today!</legend>

                        <!-- Network ID-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  >NTID</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-screenshot"></i></span>
                                    <input id="txt_ntid" name="ntid_name" placeholder="Enter Network ID" class="form-control" type="text" ng-model="ntid"/>
                                   
                                </div>
                            </div>
                        </div>

                        <!-- First Name-->

                        <div class="form-group">
                            <label class="col-md-4 control-label"  >First Name</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                    <input id="txt_fname" name="first_name" placeholder="John" class="form-control" type="text"    ng-model="firstName" />
                                </div>
                            </div>
                        </div>

                        <!-- Last Name-->

                        <div class="form-group">
                            <label class="col-md-4 control-label"  >Last Name</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                    <input id="txt_lname" name="last_name" placeholder="Doe" class="form-control" type="text"    ng-model="lastName"/>
                                </div>
                            </div>
                        </div>

                        <!-- Role ID  Hidden parameter-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  >Role Id</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                    <select name="role_id" class="form-control" ng-model="roleId">       
                                        <option value="">Select Role</option>
                                        <option value="200">Team Member</option>
                                        <option value="201">Project Manager</option>
                                    </select>
                                 
                                </div>
                            </div>
                        </div>


                        <!-- Phone no -->

                        <div class="form-group">
                            <label class="col-md-4 control-label"  >Phone #</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-earphone"></i></span>
                                    <input id="txt_phone" name="phone" placeholder="(123)456-7890" class="form-control" type="text"   ng-model="phoneNo"/>
                                </div>
                            </div>
                        </div>

                        <!--Email field-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  >E-Mail</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-envelope"></i></span>
                                    <input id="txt_email" name="email" placeholder="example@domain.com" class="form-control" type="text"  ng-model="emailId"/>
                                </div>
                            </div>
                        </div>

                        <!-- Password-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  >Password</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="txt_password" name="password" placeholder="Enter Password" class="form-control" type="password"   ng-model="password" />
                                </div>
                            </div>
                        </div>


                        <!-- Confirm Password field-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  >Confirm Password</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="txt_confpassword" name="confpassword" placeholder="Re-enter Password" class="form-control" type="password" ng-model="confPassword" />
                                </div>
                            </div>
                        </div>

                        <!--Buttons-->
                        <div class="form-group">
                            <label class="col-md-4 control-label"  ></label>
                            <div class="col-md-4" style="display: flex">


                                <button class="button" style="width: 180px"  id="btnCreateAccount"  ng-click="createAccount()">Create Account <span class="glyphicon glyphicon-send"></span></button>
                                <button class="button" style="" onclick="window.location.href='signin'">Cancel<span class="glyphicon glyphicon-remove"></span></button>
                                <button type="reset" class="button" style="margin-left: 10px; width: 180px" onclick="$('#register_form').data('formValidation').resetForm();"  >Reset Form <span class="glyphicon glyphicon-refresh"></span></button>
                                

                            </div>
                            <div>
                            
                            </div>
                        </div>

                    </fieldset>
                </div>

                <div class="col-8">
                    



                </div>
                <div class="col-2">
                    <!--Empty Section-->

                </div>
            </div>



        </div>


      <div class="modal fade" id="statusModal" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
            
            <h4 class="modal-title" id="myModalLabel" style="text-align: center">Notification</h4>
            </div>
            <div class="modal-body">
                <h3 style="text-align: center" ng-bind="message"></h3>
            </div>
            <div class="modal-footer">
                 <button class="button" style="" ng-show="isSuccess" onclick="goToURL('signin')">Sign in</button>
                <button class="button" style="" data-dismiss="modal" ng-hide ="isSuccess">Close</button>
                
        </div>
    </div>
  </div>
</div>



    </form>

    <div id="myModal" class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
  <div class="modal-dialog modal-sm" role="document">
    <div class="modal-content">
      <label ng-model="status"> </label>
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
