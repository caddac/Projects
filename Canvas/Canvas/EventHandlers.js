//event handlers for page

//mouse is moved on staticCanvas
function mouseMove(event) {
    mouseLoc = getPosition(event);
    mouseMoveLocation(mouseLoc);
}

//mouse is moved on staticCanvas
function mouseMoveLocation(mousePosition) {


    if (!(dynamicShapes.length == 0 && staticShapes.length == 0)) {
        if (dynamicShapes.length != 0) {
            if (dynamicShapes[0].shape == "group")
                var closestPoint = dynamicShapes[0].s[0].p[0];
            else
            var closestPoint = dynamicShapes[0].p[0];
        } else {
            if (staticShapes[0].shape == "group")
                var closestPoint = staticShapes[0].s[0].p[0];
        else
            var closestPoint = staticShapes[0].p[0];
        }

            for (var a = 0; a < staticShapes.length; a++) {
                for (var b = 0; b < staticShapes[a].n; b++) {
                    if (staticShapes[a].shape == "ellipse") {
                        if (dist(staticShapes[a].p[0], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = staticShapes[a].p[0];
                    }
                    else if (staticShapes[a].shape == "text") {
                        if (dist(staticShapes[a].p[0], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = staticShapes[a].p[0];
                    }
                    else if (staticShapes[a].shape == "group") {
                        for (k = 0; k < staticShapes[a].s[b].n; k++) {
                            if (staticShapes[a].s[b].shape == "ellipse") {
                                if (dist(staticShapes[a].s[b].p[0], mousePosition) < dist(closestPoint, mousePosition))
                                    closestPoint = staticShapes[a].s[b].p[0];
                            }
                            else {
                                if (dist(staticShapes[a].s[b].p[k], mousePosition) < dist(closestPoint, mousePosition))
                                    closestPoint = staticShapes[a].s[b].p[k];
                            }
                        }
                    }
                    else
                        if (dist(staticShapes[a].p[b], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = staticShapes[a].p[b];
                }
            }




            for (var a = 0; a < dynamicShapes.length; a++) {
                for (var b = 0; b < dynamicShapes[a].n; b++) {
                    if (dynamicShapes[a].shape == "ellipse") {
                        if (dist(dynamicShapes[a].p[0], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = dynamicShapes[a].p[0];
                    }
                    else if (dynamicShapes[a].shape == "text") {
                        if (dist(dynamicShapes[a].p[0], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = dynamicShapes[a].p[0];
                    }
                    else if (dynamicShapes[a].shape == "group") {
                        for (k = 0; k < dynamicShapes[a].s[b].n; k++) {
                            if (dynamicShapes[a].s[b].shape == "ellipse") {
                                if (dist(dynamicShapes[a].s[b].p[0], mousePosition) < dist(closestPoint, mousePosition))
                                    closestPoint = dynamicShapes[a].s[b].p[0];
                            }
                            else {
                                if (dist(dynamicShapes[a].s[b].p[k], mousePosition) < dist(closestPoint, mousePosition))
                                closestPoint = dynamicShapes[a].s[b].p[k];
                        }
                    }
                    }
                    else
                        if (dist(dynamicShapes[a].p[b], mousePosition) < dist(closestPoint, mousePosition))
                            closestPoint = dynamicShapes[a].p[b];
                }
            }

        //refresh top staticCanvas
            topCanvas.width = topCanvas.width;

            if (selecting) {
                topctx.beginPath();
                topctx.strokeStyle = red;
                topctx.moveTo(clickDownLocation.x, clickDownLocation.y);
                topctx.lineTo(clickDownLocation.x, mousePosition.y);
                topctx.lineTo(mousePosition.x, mousePosition.y);
                topctx.lineTo(mousePosition.x, clickDownLocation.y);
                topctx.lineTo(clickDownLocation.x, clickDownLocation.y);
                topctx.closePath();
                topctx.stroke();
            }

            //draw red line
            topctx.beginPath();
            topctx.strokeStyle = red;
            topctx.moveTo(mousePosition.x, mousePosition.y);
            topctx.lineTo(closestPoint.x, closestPoint.y);
            topctx.closePath();
            topctx.stroke();
    }
    return closestPoint;
}

//mouse button is pressed down
function mousedown(event) {
    clickDownLocation = getPosition(event);
    if (setAction() == 8)
        selecting = true;

    // This rounding was critical to debugging for points on the hull
    clickDownLocation.x = 10 * Math.round(clickDownLocation.x / 10);
    clickDownLocation.y = 10 * Math.round(clickDownLocation.y / 10);

}

//mouse button is released
function mouseup(event) {
    clickUpLocation = getPosition(event);
    selecting = false;

    // This rounding was critical to debugging for points on the hull
    //this rounding was causing the position to be NaN
    clickUpLocation.x = 10 * Math.round(clickUpLocation.x / 10);
    clickUpLocation.y = 10 * Math.round(clickUpLocation.y / 10);
    doAction();//main call to start doing some stuff
}

function arrowhandle(event) {
    if(event.shiftKey){
        shiftPressed = true;
    }
    else {
    if( !typing ) {
        if (event.which == 37) {
            nudgeSelection(dynamicShapes,-1,0);
        }
        else if (event.which == 38) {
            nudgeSelection(dynamicShapes, 0, -1);
        }
        else if (event.which == 39) {
            nudgeSelection(dynamicShapes, 1, 0);
        }
        else if (event.which == 40) {
            nudgeSelection(dynamicShapes, 0, 1);
        }
        restore(dynctx, dynamicShapes, dynCanvas, red, true);
        restore(staticCtx, staticShapes, staticCanvas, black, true);
        mouseMoveLocation(mouseLoc);
    }
}
}

function saveData() {
    var myarray;
    var myJSON = "";

    myarray = { stat: staticShapes, dyn: dynamicShapes };
    myJSON = JSON.stringify({ myarray: myarray });
    document.getElementById('outputText').innerHTML = myJSON;
}

function loadData() {
    var myarray;
    var myJSON = prompt("Enter your data below:");

    myarray = JSON.parse(myJSON);
    if (typeof myarray.myarray.stat != 'undefined')
        staticShapes = myarray.myarray.stat;
    else
        staticShapes = [];
    dynamicShapes = myarray.myarray.dyn;
    restore(staticCtx, staticShapes, staticCanvas, "black", true);
    restore(dynctx, dynamicShapes, dynCanvas, "red", true );
}

function clearAll() {
    staticCanvas.width = staticCanvas.width;
    dynCanvas.width = dynCanvas.width;
    topCanvas.width = topCanvas.width;
    dynamicShapes.length = 0;
    staticShapes.length = 0;
}

function insertItem() {
    var myarray;
    var myJSON = prompt("Enter your data below:");

    myarray = JSON.parse(myJSON);
    dynamicShapes = myarray.myarray.dyn;
    restore(dynctx, dynamicShapes, dynCanvas, "red", true);
}

function saveSelection() {
    var myarray;
    var myJSON = "";

    myarray = { dyn: dynamicShapes };
    myJSON = JSON.stringify({ myarray: myarray });
    document.getElementById('outputText').innerHTML = myJSON;
}

function shiftOff(event) {
    if (!event.shiftKey) {
        shiftPressed = false;
    }
}

function sharesPoint(shape1, shape2) {
    for (var i = 0; i < shape1.n; i++) {
        for (var j = 0; j < shape2.n; j++) {
            if ((shape1.p[i].x == shape2.p[j].x) && (shape1.p[i].y == shape2.p[j].y))
                return true;
        }
    }
    return false;
}

function propagate() {
    //For every shape in dynamic array:
    //If shape is line: check if shape shares same point
    //If shape is shape: check if line shares same point
    //For either case splice static array and put into dynamic array
    var len = dynamicShapes.length;
    if (dynamicShapes.length == 0)
        return;
    for (var i = 0; i < len; i++) {
        switch (dynamicShapes[i].shape) {
            case "group":
                for (var j = 0; j < dynamicShapes[i].n; j++) {//For every shape in group
                    if (dynamicShapes[i].s[j].shape != "line") {//Don't need to check the lines
                        for (var k = 0; k < staticShapes.length; k++) {//Check every static shape
                            if (sharesPoint(dynamicShapes[i].s[j], staticShapes[k])) {
                                tempshape = staticShapes.splice(k, 1);
                                dynamicShapes.push(tempshape[0]);
                                j--;
                            }
                        }
                    }
                }
                break;
            case "line":
                for( var j=0; j < staticShapes.length; j++ ) {
                    if (staticShapes[j].shape != "line") {
                        if (staticShapes[j].shape == "group") {
                            for (var k = 0; k < staticShapes[j].n; k++) {
                                if (sharesPoint(dynamicShapes[i], staticShapes[j].s[k])) {
                                    tempshape = staticShapes.splice(j, 1);
                                    dynamicShapes.push(tempshape[0]);
                                    j--;
                                    break;
                                }
                            }
                        } else {
                            if (sharesPoint(dynamicShapes[i], staticShapes[j])) {
                                tempshape = staticShapes.splice(j, 1);
                                dynamicShapes.push(tempshape[0]);
                                j--;
                            }
                        }
                    }
                }
                break;
            default:
                for (var j = 0; j < staticShapes.length; j++) {
                    if (staticShapes[j].shape == "line") {
                        if (sharesPoint(dynamicShapes[i], staticShapes[j])) {
                            tempshape = staticShapes.splice(j, 1);
                            dynamicShapes.push(tempshape[0]);
                            j--;
                            //i--;
                        }
                    }
                }
                break;
        }
    }
    dynCanvas.width = dynCanvas.width;
    staticCanvas.width = staticCanvas.width;
    restore(staticCtx, staticShapes, staticCanvas, black, true);
    restore(dynctx, dynamicShapes, dynCanvas, red, true);
}

//when keyboard is pressed
function keyhandle(event) {
    var i;
    event = event || window.event;
    if (!typing) // Must click mouse first
        return;
    if (event.which == 37 || event.which == 38 || event.which == 39 || event.which == 40)//These should be handled above not here
        return;
    if (event.which == 13) // Detect <enter>
    {
        typing = false;
        textBuffer =
        textBuffer.slice(0, -1); // Remove cursor
        if (textBuffer.length == 0)
            alert("Empty text ignored");
    }
    else if (event.which == 8) {
        // Process <delete> by removing the symbol typed last & reappending cursor
        if (textBuffer.length == 1 && textBuffer[textBuffer.length-1] == '_')
            alert("Hey Einstein, your trying to delete the cursor!");
        else {
            textBuffer =
            textBuffer.slice(0, -2) + '_';
        }
        event.preventDefault();
        event.stopPropagation();
    }
    else {
        // Remove cursor, append new symbol and cursor
        
        if (event.shiftKey) {
            textBuffer =
            textBuffer.slice(0, -1) +
            String.fromCharCode(event.which) + '_';
        }
        else {
            textBuffer =
            textBuffer.slice(0, -1) +
            String.fromCharCode(event.which).toLowerCase() + '_';
        }

    }
    // Redraw entire
    dynctx.clearRect(0, 0, 500, 500);

    if (event.which == 13) {    //Enter
        staticCtx.textAlign = "center";
        staticCtx.font = "10pt Geneva";
        size = dynctx.measureText(textBuffer).width/2;
        if (clickUpLocation.x + size > 500 || clickUpLocation.x - size < 0) {
            textBuffer = [];
            return;
        }
        else {
            //staticCtx.fillText(textBuffer, typingLocation.x, typingLocation.y);
            staticShapes.push(new Text("text", 2, [{ x: typingLocation.x - size, y: typingLocation.y },
                                                   { x: typingLocation.x + size, y: typingLocation.y-12}], textBuffer));
            textBuffer = [];
        }
    } else {
        dynctx.fillStyle = red;
        dynctx.textAlign = "center";
        dynctx.font = "10pt Geneva";
        dynctx.fillText(textBuffer, typingLocation.x, typingLocation.y);
    }
}

function deleteItem() {
    var lineConnected;
    for (var i = 0; i < dynamicShapes.length; i++) {
        if (dynamicShapes[i].shape == "group") {
            for (var j = 0; j < dynamicShapes[i].n; j++) {
                lineConnected = isLineConnected(dynamicShapes[i])
                if (isLineConnected(dynamicShapes[i].s[j]))
                    return;
            }
        }
        else {
            lineConnected = isLineConnected(dynamicShapes[i])
            if (isLineConnected(dynamicShapes[i]))
                return;
        }
    }

    dynamicShapes.length = 0;
    dynCanvas.width = dynCanvas.width;
    restore(staticCtx, staticShapes, staticCanvas, black, true);
    topCanvas.width = topCanvas.width;
}

function isLineConnected(shape){
    var point = shape.p;
    var newN = shape.n;

    for (var a = 0; a < newN; a++) {//every point of the incoming shape
        for (var b = 0; b < staticShapes.length; b++) {//for every object in the static shape array
            switch (staticShapes[b].shape) {
                case "group":
                    break;
                case "line":
                    for (var c = 0; c < staticShapes[b].n; c++) {
                        if (staticShapes[b].p[c] == shape.p[a]) {
                            return true;
                        }
                    }
                    break;
                default:


            }
        }
        return false;
    }
}