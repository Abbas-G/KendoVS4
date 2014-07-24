        function fadeinout() {
            var popID = "popup_name"; //Get Popup Name
            var popURL = "?w=90";  //Get Popup href to define size

            var query = popURL.split('?');
            var dim = query[1].split('&');
            var popWidth = dim[0].split('=')[1]; //Gets the first query string value

            //Fade in the Popup and add close button
            $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"></a>');
            // $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"><img src="images/close_btn.png" class="btn_close" title="Close Window" alt="Close" /></a>');
            var popMargTop = ($('#' + popID).height() + 20) / 2;
            var popMargLeft = ($('#' + popID).width() + 80) / 2;

            //Apply Margin to Popup
            $('#' + popID).css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            //Fade in Background
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=80)' }).fadeIn();

        }
        function fadeover() {
            $('#fade , .popup_block').fadeOut(function() {
                $('#fade, a.close').remove();  //fade them both out

            });
        }



        function PopUp(imgsrc, imgcls) {
            //alert("sss");
            var popID = "popup_portfolio"; //Get Popup Name
            var popURL = "?w=580";  //Get Popup href to define size
            //document["PLargeImg"].src = imgsrc;
            document.getElementById("PLargeImg").src = imgsrc;
            var query = popURL.split('?');
            var dim = query[1].split('&');
            var popWidth = dim[0].split('=')[1]; //Gets the first query string value

            //Fade in the Popup and add close button
            $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"></a>');
            //$('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" onclick="PopOut()" class="close"><img src=' + imgcls + ' title="Close Window" alt="Close" /></a>');
            // $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"><img src="images/close_btn.png" class="btn_close" title="Close Window" alt="Close" /></a>');
            var popMargTop = ($('#' + popID).height() + 20) / 2;
            var popMargLeft = ($('#' + popID).width() + 80) / 2;

            //Apply Margin to Popup
            $('#' + popID).css({
                'margin-top': -popMargTop,
                'margin-left': -popMargLeft
            });

            //Fade in Background
            $('body').append('<div id="fade" onclick="PopOut()" ></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=80)' }).fadeIn();

        }
        function PopOut() {
            $('#fade , .popup_porfolio').fadeOut(function() {
                $('#fade, a.close').remove();  //fade them both out

            });
        }
        