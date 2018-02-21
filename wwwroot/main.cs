



/*//////////////////////////////////////////////////////////////////
[ FONT ]*/

@font-face {
  font-family: Montserrat-Regular;
  src: url('../fonts/montserrat/Montserrat-Regular.ttf'); 
}

@font-face {
  font-family: Montserrat-ExtraBold;
  src: url('../fonts/montserrat/Montserrat-ExtraBold.ttf'); 
}

@font-face {
  font-family: Montserrat-Bold;
  src: url('../fonts/montserrat/Montserrat-Bold.ttf'); 
}


/*//////////////////////////////////////////////////////////////////
[ RESTYLE TAG ]*/

* {
	margin: 0px; 
	padding: 0px; 
	box-sizing: border-box;
}

body, html {
	height: 100%;
	font-family: Montserrat-Regular, sans-serif;
}

/*---------------------------------------------*/
a {
	font-family: Montserrat-Regular;
	font-size: 14px;
	line-height: 1.7;
	color: #666666;
	margin: 0px;
	transition: all 0.4s;
	-webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
}

a:focus {
	outline: none !important;
}

a:hover {
	text-decoration: none;
  color: #fff;
}

/*---------------------------------------------*/
h1,h2,h3,h4,h5,h6 {
	margin: 0px;
}

p {
	font-family: Montserrat-Regular;
	font-size: 14px;
	line-height: 1.7;
	color: #666666;
	margin: 0px;
}

ul, li {
	margin: 0px;
	list-style-type: none;
}


/*---------------------------------------------*/
input {
	outline: none;
	border: none;
}

textarea {
  outline: none;
  border: none;
}

textarea:focus, input:focus {
  border-color: transparent !important;
}

input:focus::-webkit-input-placeholder { color:transparent; }
input:focus:-moz-placeholder { color:transparent; }
input:focus::-moz-placeholder { color:transparent; }
input:focus:-ms-input-placeholder { color:transparent; }

textarea:focus::-webkit-input-placeholder { color:transparent; }
textarea:focus:-moz-placeholder { color:transparent; }
textarea:focus::-moz-placeholder { color:transparent; }
textarea:focus:-ms-input-placeholder { color:transparent; }

input::-webkit-input-placeholder { color: #999999; }
input:-moz-placeholder { color: #999999; }
input::-moz-placeholder { color: #999999; }
input:-ms-input-placeholder { color: #999999; }

textarea::-webkit-input-placeholder { color: #999999; }
textarea:-moz-placeholder { color: #999999; }
textarea::-moz-placeholder { color: #999999; }
textarea:-ms-input-placeholder { color: #999999; }

/*---------------------------------------------*/
button {
	outline: none !important;
	border: none;
	background: transparent;
}

button:hover {
	cursor: pointer;
}

iframe {
	border: none !important;
}


/*//////////////////////////////////////////////////////////////////
[ Utility ]*/
.txt1 {
  font-family: Montserrat-Regular;
  font-size: 16px;
  color: #cccccc;
}

.bo1 {
  border-bottom: 1px solid #999999;
}

.hov1:hover {
  border-color: #d33f8d;
}


/*//////////////////////////////////////////////////////////////////
[ login ]*/

.limiter {
    margin-top:-100px;
  width: 100%;
  height: 100%;
  background-color:silver;
  color:silver;
}

.container-login100 {
    width: 100%;
    height: 100%;
    min-height: 0vh;
    display: -webkit-box;
    display: -webkit-flex;
    display: -moz-box;
    display: -ms-flexbox;
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    align-items: center;
    padding: 0px;
    background-repeat: no-repeat;
    background-size: auto;
    background-position: center;
    background-color: silver;
    margin-top: 100px;
    position: relative;
    z-index: 1;
    top: 0px;
    left: 0px;
    color:silver;
}

.container-login100::before {
  content: "";
  display: block;
  position: absolute;
  z-index: -1;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  background: #005bea;
  background: -webkit-linear-gradient(bottom, #005bea, #00c6fb);
  background: -o-linear-gradient(bottom, #005bea, #00c6fb);
  background: -moz-linear-gradient(bottom, #005bea, #00c6fb);
  background: linear-gradient(bottom, #005bea, #00c6fb);
  opacity: 0.2;
}

.wrap-login100 {
    margin-top:-300px;
  width: 300px;
  height:200px;
  }



/*------------------------------------------------------------------
[  ]*/
.login100-form {
  width: 100%;
  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
}

.login100-form-title {
  font-family: Montserrat-ExtraBold;
  font-size: 45px;
  color: #fff;
  line-height: 1.2;
  text-align: center;
  width: 100%;
  display: block;
}



/*---------------------------------------------*/
.wrap-input100 {
  position: relative;
  width: 80%;
  z-index: 3;
  /*border: 1px solid silver;*/
  margin-left:auto;
  margin-right:auto;
}

.input100 {
  font-family: Montserrat-Bold;
  font-size: 15px;
  line-height: 1.2;
  color: silver;

  display: block;
  width: 100%;
  background: #fff;
  height: 50px;
  border-radius: 25px;
  padding: 0 30px 0 53px;
}


/*------------------------------------------------------------------
[ Focus ]*/
.focus-input100 {
  display: block;
  position: absolute;
  border-radius: 25px;
  bottom: 0;
  left: 0;
  z-index: -1;
  width: 100%;
  height: 100%;
  box-shadow: 0px 0px 0px 0px;
  color: rgba(0,91,234, 0.6);
}

.input100:focus + .focus-input100 {
  -webkit-animation: anim-shadow 0.5s ease-in-out forwards;
  animation: anim-shadow 0.5s ease-in-out forwards;
}

@-webkit-keyframes anim-shadow {
  to {
    box-shadow: 0px 0px 80px 30px;
    opacity: 0;
  }
}

@keyframes anim-shadow {
  to {
    box-shadow: 0px 0px 80px 30px;
    opacity: 0;
  }
}

.symbol-input100 {
  font-size: 15px;
  color: #999999;

  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  align-items: center;
  position: absolute;
  border-radius: 25px;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 100%;
  padding-left: 30px;
  pointer-events: none;

  -webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
  transition: all 0.4s;
}

.input100:focus + .focus-input100 + .symbol-input100 {
  color: #00c6fb;
  padding-left: 23px;
}


/*------------------------------------------------------------------
[ Button ]*/
.container-login100-form-btn {
  width: 80%;
  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-left:auto;
  margin-right:auto;
}

.login100-form-btn {
  font-family: Montserrat-Bold;
  font-size: 15px;
  line-height: 1.5;
  color: #e0e0e0;

  width: 100%;
  height: 50px;
  border-radius: 25px;
  background: #333333;
  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0 25px;

  -webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
  transition: all 0.4s;

  position: relative;
  z-index: 1;
}

.login100-form-btn::before {
  content: "";
  display: block;
  position: absolute;
  z-index: -1;
  width: 100%;
  height: 100%;
  border-radius: 25px;
  top: 0;
  left: 0;
  background: #005bea;
  background: -webkit-linear-gradient(left, #005bea, #00c6fb);
  background: -o-linear-gradient(left, #005bea, #00c6fb);
  background: -moz-linear-gradient(left, #005bea, #00c6fb);
  background: linear-gradient(left, #005bea, #00c6fb);
  -webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
  transition: all 0.4s;
  opacity: 0;
}

.login100-form-btn:hover {
  background: transparent;
  color: #fff;
}

.login100-form-btn:hover:before {
  opacity: 1;
}


/*//////////////////////////////////////////////////////////////////
[ responsive ]*/

@media (max-width: 500px) {
  .wrap-login100 {
    padding-top: 10px;
    padding-left: 15px;
    padding-right: 15px;
  }
}
