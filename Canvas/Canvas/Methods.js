//canvas variables
var staticCanvas, staticCtx;
var topCanvas, topctx;
var dynCanvas, dynctx;
var gridCanvas, gridCtx;
//positioning variables used for the canvases
var offset_left = 0;
var offset_top = 0;
var offset_left1 = 0;
var offset_top1 = 0;
var offset_left2 = 0;
var offset_top2 = 0;
var offset_left3 = 0;
var offset_top3 = 0;
var mouseLoc;
//other variabls
var origin = { x: 0, y: 0 };
var typing = false;
var black = "black";
var red = "red";
var onEdge = false;
var shiftPressed = false;
var selecting = false;
var shapeToDraw;
var clickDownLocation, clickUpLocation;
//holds shapes on the static canvas
var staticShapes = [];
//holds shapes on the dynamic canvas
var dynamicShapes = [];
//holds all of the points currently on any canvas
var allPointArray = [];
//a temporary array for building point arrays for a single shape
var pointArray = [];
//holds all of the lines on the static canvas
var staticLines = [];
//holds all of the lines on the dynamic canvas
var dynamicLines = []
//holds the text being written right now
var textBuffer = [];
//screen coordinates for each text item
var typingLocation = 0;

//methods

//shape Factory
function Shape(shape, n, p) {

    this.shape = shape;
    this.n = n;
    this.p = p;
    return this;
    
}

//text factory
function Text(shape, n, p, text) {
    this.shape = shape;
    this.n = n;
    this.p = p;
    this.text = text;
}

//group factory
function Group(shape, n, s) {
    this.shape = shape;
    this.n = n;
    this.s = s;
}

//figure out what action to do and do it
function doAction() {
    setAction();
    switch (shapeToDraw) {
        case 1://square
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);
            drawSquare();
            break;
        case 2://circle
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawCircle();
            break;
        case 3://triangle
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawTriangle();
            break;
        case 4://ellipse
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawEllipse();
            break;
        case 5://line
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawLine();
            break;
        case 6://text
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawText();
            break;
        case 7://polys
            staticShapes = staticShapes.concat(dynamicShapes);
            dynamicShapes.length = 0;
            restore(staticCtx, staticShapes, staticCanvas, black, true);
            restore(dynctx, dynamicShapes, dynCanvas, red, true);

            drawPoly();
            break;
        case 8:
            makeSelection();
            break;
        case 9:
            singleSelect();
            break;
        case 10:    //Move
            vectorMove();
            break;
    }//end switch

}

//sets which action is selected on page
function setAction() {
    var radios = document.getElementsByName('shapeRdoBtn');

    for (var i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {
            shapeToDraw = i + 1;
            return i + 1;//returns index + 1, which is the value, just have to make sure they line up on the html side
        }
    }
}

//translates on a rectangular coordinate systemm point Pt around point A, theta degrees
function translate(Pt, A, theta) {
    if (!(theta < 0 || theta > 0))
        return Pt;
    return { x: Math.cos(theta) * (Pt.x - A.x) - Math.sin(theta) * (Pt.y - A.y) + A.x, y: Math.sin(theta) * (Pt.x - A.x) + Math.cos(theta) * (Pt.y - A.y) + A.y };
}

//calculates distance p to q
function dist(p, q) {
    var dx = q.x - p.x, dy = q.y - p.y;
    return Math.sqrt(dx * dx + dy * dy);
}

//gets the location of an event
function getPosition(evt) {
    evt = (evt) ? evt : ((event) ? event : null);
    var left = 0;
    var top = 0;

    // Make sure coordinates mean something
    if (evt.pageX) {
        left = evt.pageX;
        top = evt.pageY;
    } else if (document.documentElement.scrollLeft) {
        left = evt.clientX + document.documentElement.scrollLeft;
        top = evt.clientY + document.documentElement.scrollTop;
    } else {
        left = evt.clientX + document.body.scrollLeft;
        top = evt.clientY + document.body.scrollTop;
    }
    left -= offset_left;
    top -= offset_top;

    return { x: left, y: top };
}

//wipes and restores a canvas given a shapeArray
function restore(cntx, arr, canvas, color, clear) {
    if( clear )
        canvas.width = canvas.width;
    var len = arr.length;
    for (var i = 0; i < len; i++) {
        cntx.strokeStyle = color;
        cntx.beginPath();
        if (arr[i].shape == "circle") {
            var cent = arr[i].p[0];
            var rad = arr[i].p[1];
            //drawPoint(cent, black);
            cntx.arc(cent.x, cent.y, rad, 0, 2 * Math.PI, true);
        }
        else if (arr[i].shape == "text") {
            cntx.fillStyle = color;
            cntx.textAlign = "left";
            cntx.font = "10pt Geneva";
            cntx.fillText(arr[i].text, arr[i].p[0].x, arr[i].p[0].y);
        }
        else if (arr[i].shape == "group") {
            restore(cntx,arr[i].s,canvas,color, false);
        }
        else if (arr[i].shape == "ellipse") {
            var ellCent = arr[i].p[0];
            var down = arr[i].p[1];
            var up = arr[i].p[2];

            cntx.moveTo(down.x, ellCent.y);
            cntx.quadraticCurveTo(down.x, down.y, ellCent.x, down.y);
            cntx.quadraticCurveTo(up.x, down.y, up.x, ellCent.y);
            cntx.quadraticCurveTo(up.x, up.y, ellCent.x, up.y);
            cntx.quadraticCurveTo(down.x, up.y, down.x, ellCent.y);
        } else {
            var numPoints = arr[i].n;
            cntx.moveTo(arr[i].p[0].x, arr[i].p[0].y);
            for (var j = 1; j < numPoints; j++) {
                var pnt = arr[i].p[j];
                cntx.lineTo(pnt.x, pnt.y);
            }
        }
        cntx.closePath();
        cntx.stroke();
    }
    drawAllPoints(cntx, arr, color);
}

function within(shape, A, B)
{
    if (shape.shape == "circle") {
        if (!(
            ((shape.p[0].x > A.x && shape.p[0].x < B.x) && (shape.p[0].y > A.y && shape.p[0].y < B.y)) ||
            ((shape.p[0].x < A.x && shape.p[0].x > B.x) && (shape.p[0].y < A.y && shape.p[0].y > B.y)) ||
            ((shape.p[0].x > A.x && shape.p[0].x < B.x) && (shape.p[0].y < A.y && shape.p[0].y > B.y)) ||
            ((shape.p[0].x < A.x && shape.p[0].x > B.x) && (shape.p[0].y > A.y && shape.p[0].y < B.y))
            )) return false;//we're done
    }
    else if (shape.shape == "group") {
        for (var b = 0; b < shape.n; b++) {//for every shape in the group
            if (!(within(shape.s[b], A, B)))
                return false;
        }
    }
    else {
        for (var a = 0; a < shape.n; a++) {
            //if any point is not in the box
            if (!(
                ((shape.p[a].x > A.x && shape.p[a].x < B.x) && (shape.p[a].y > A.y && shape.p[a].y < B.y)) ||
                ((shape.p[a].x < A.x && shape.p[a].x > B.x) && (shape.p[a].y < A.y && shape.p[a].y > B.y)) ||
                ((shape.p[a].x > A.x && shape.p[a].x < B.x) && (shape.p[a].y < A.y && shape.p[a].y > B.y)) ||
                ((shape.p[a].x < A.x && shape.p[a].x > B.x) && (shape.p[a].y > A.y && shape.p[a].y < B.y))
                ))
                return false;//we're done
        }
    }
    return true;

}

//draws a point to the staticCanvas
function drawPoint(loc, color, cont) {
    cont.beginPath();
    cont.arc(loc.x, loc.y, 3, 0, Math.PI * 2, true);
    cont.closePath();
    cont.fillStyle = color;
    cont.fill();
}
function drawTriangle() {
    A = { x: clickDownLocation.x, y: clickDownLocation.y };
    B = { x: clickUpLocation.x, y: clickUpLocation.y };
    len = dist(A, B);

    if (A.x < B.x) //Left to Right
        Bprime = { x: A.x + len, y: A.y };
    else           //Right to Left
        Bprime = { x: A.x - len, y: A.y };

    thetar = Math.acos(((Math.pow(dist(Bprime, B), 2)) - (Math.pow(len, 2)) - (Math.pow(len, 2))) / (-2 * len * len));
    height = len * ((Math.sqrt(3)) / 2);

    if ((A.x < B.x)) //Left to Right
        mid = ((Bprime.x - A.x) / 2) + A.x;
    else             //Right to Left
        mid = ((A.x - Bprime.x) / 2) + Bprime.x;

    if ((A.x < B.x) && (A.y < B.y)) {
        Pt = { x: mid, y: (A.y - height) };
        P = translate(Pt, A, thetar);
    }
    else if ((B.x < A.x) && (B.y < A.y)) {
        Pt = { x: mid, y: (A.y + height) };
        P = translate(Pt, A, thetar);
    }
    else if ((B.x < A.x) && (A.y < B.y)) {
        Pt = { x: mid, y: (A.y + height) };
        P = translate(Pt, A, -thetar);
    }
    else {
        if (A.y < B.y) {
            Pt = { x: mid, y: (A.y + height) };
            P = translate(Pt, A, -thetar);
        }
        else if (A.x < B.x) {
            Pt = { x: mid, y: (A.y - height) };
            P = translate(Pt, A, -thetar);
        }
        else {
            Pt = { x: mid, y: (A.y + height) };
            P = translate(Pt, A, thetar);
        }
    }

    if (A.x > 500 || A.y > 500 || B.x > 500 || B.y > 500 || P.x > 500 || P.y > 500 ||
        A.x < 0 || A.y < 0 || B.x < 0 || B.y < 0 || P.x < 0 || P.y < 0)
        return;

    staticCtx.beginPath();
    staticCtx.strokeStyle = black;

    staticCtx.moveTo(A.x, A.y);
    staticCtx.lineTo(B.x, B.y);
    staticCtx.lineTo(P.x, P.y);
    staticCtx.lineTo(A.x, A.y);
    staticCtx.closePath();
    staticCtx.stroke();
    drawPoint(A, black, staticCtx);
    drawPoint(B, black, staticCtx);
    drawPoint(P, black, staticCtx);

    pointArray = new Array(A, B, P);
    staticShapes.push( new Shape("triangle", 3, pointArray));

}
function drawSquare() {
    staticCtx.strokeStyle = black;
    staticCtx.strokeRect(clickDownLocation.x, clickDownLocation.y, clickUpLocation.x - clickDownLocation.x, clickUpLocation.y - clickDownLocation.y);
    //draw points on corners
    drawPoint(clickDownLocation, black, staticCtx);
    drawPoint(clickUpLocation, black, staticCtx);
    var c1 = { x: clickDownLocation.x, y: clickUpLocation.y }, c2 = { x: clickUpLocation.x, y: clickDownLocation.y }
    drawPoint(c1, black, staticCtx);
    drawPoint(c2, black, staticCtx);
    //add shape to shape array for count later
    pointArray = new Array(clickDownLocation, c2, clickUpLocation, c1);
    staticShapes.push(new Shape("square", 4, pointArray));

}
function drawCircle() {
    staticCtx.beginPath();
    staticCtx.strokeStyle = black;
    var radius = dist(clickDownLocation, clickUpLocation);

    dist1 = dist({ x: clickDownLocation.x, y: 0 }, clickDownLocation);
    dist2 = dist({ x: clickDownLocation.x, y: 500 }, clickDownLocation);
    dist3 = dist({ x: 0, y: clickDownLocation.y }, clickDownLocation);
    dist4 = dist({ x: 500, y: clickDownLocation.y }, clickDownLocation);

    if (radius > dist1 || radius > dist2 || radius > dist3 || radius > dist4)
        return;

    staticCtx.arc(clickDownLocation.x, clickDownLocation.y, radius, 0, 2 * Math.PI, true);
    staticCtx.stroke();
    //draw center point
    drawPoint(clickDownLocation, black, staticCtx);//center point
    //add shape to shape array
    
    pointArray = new Array(clickDownLocation,radius);
    staticShapes.push(new Shape("circle", 2, pointArray));

}
function drawEllipse() {
    var centery = clickDownLocation.y + (clickUpLocation.y - clickDownLocation.y) / 2;
    var centerx = clickDownLocation.x + (clickUpLocation.x - clickDownLocation.x) / 2;

    staticCtx.beginPath();
    staticCtx.strokeStyle = black;

    staticCtx.moveTo(clickDownLocation.x, centery);
    staticCtx.quadraticCurveTo(clickDownLocation.x, clickDownLocation.y, centerx, clickDownLocation.y);
    staticCtx.quadraticCurveTo(clickUpLocation.x, clickDownLocation.y, clickUpLocation.x, centery);
    staticCtx.quadraticCurveTo(clickUpLocation.x, clickUpLocation.y, centerx, clickUpLocation.y);
    staticCtx.quadraticCurveTo(clickDownLocation.x, clickUpLocation.y, clickDownLocation.x, centery);
    staticCtx.stroke();
    staticCtx.closePath();
    //draw point
    var center = { x: clickDownLocation.x + (clickUpLocation.x - clickDownLocation.x) / 2, y: centery };
    drawPoint(center, black, staticCtx);
    //add to shape array
    pointArray = new Array(center, clickDownLocation, clickUpLocation);
    staticShapes.push(new Shape("ellipse", 3, pointArray));

}
function drawLine() {

    if (staticShapes.length > 1)//if points on staticCanvas and more than one shape on staticCanvas
    {
        var firstpoint = origin;//assume first point is closest
        var secondpoint = origin;
        var firstshape=0, secondshape=0;

        for (var a = 0; a < staticShapes.length; a++) {
            for (var b = 0; b < staticShapes[a].n; b++) {
                switch (staticShapes[a].shape) {
                    case "group":
                        for (var c = 0; c < staticShapes[a].s[b].n; c++) {
                            if (dist(staticShapes[a].s[b].p[c], clickDownLocation) <= dist(firstpoint, clickDownLocation)) {
                                firstpoint = staticShapes[a].s[b].p[c];
                                firstshape = a;
                            }
                            //check last point
                            if (dist(staticShapes[a].s[b].p[c], clickUpLocation) <= dist(secondpoint, clickUpLocation)) {
                                secondpoint = staticShapes[a].s[b].p[c];
                                secondshape = a;
                            }
                        }
                        break;
                    case "circle":
                    case "ellipse":
                        //check center only
                        //check first click
                        if (dist(staticShapes[a].p[0], clickDownLocation) <= dist(firstpoint, clickDownLocation)) {
                            firstpoint = staticShapes[a].p[0];
                            firstshape = a;
                        }
                        //check second click
                        if (dist(staticShapes[a].p[0], clickUpLocation) <= dist(secondpoint, clickUpLocation)) {
                            secondpoint = staticShapes[a].p[0];
                            secondshape = a;
                        }
                        break;
                    case "line": break;
                    default:
                        //check first point
                        if (dist(staticShapes[a].p[b], clickDownLocation) <= dist(firstpoint, clickDownLocation)) {
                            firstpoint = staticShapes[a].p[b];
                            firstshape = a;
                        }
                        //check last point
                        if (dist(staticShapes[a].p[b], clickUpLocation) <= dist(secondpoint, clickUpLocation)) {
                            secondpoint = staticShapes[a].p[b];
                            secondshape = a;
                        }
                        break;
                }
            }
        }
        if (firstshape == secondshape)//if line is drawn from and to same shape
            return;

        for (var z = 0; z < staticShapes.length; z++) {//look through all the shapes
            if ((staticShapes[z].shape == "line")) {//if the shape is a line
                if ((staticShapes[z].p[0] == firstpoint && staticShapes[z].p[1] == secondpoint) || (staticShapes[z].p[0] == secondpoint && staticShapes[z].p[1] == firstpoint)){
                //alert("line already exists");
                return;//don't draw it
            }
        }
    }
    }

    staticCtx.beginPath();
    staticCtx.moveTo(firstpoint.x, firstpoint.y);
    staticCtx.lineTo(secondpoint.x, secondpoint.y);
    staticCtx.closePath();
    staticCtx.stroke();
    pointArray = new Array(firstpoint, secondpoint);
    staticShapes.push(new Shape("line", 2, pointArray));
    //staticLines.push({ p1: firstpoint, p2: secondpoint });

}
function drawPoly(){
    var twoPI = 2 * Math.PI;
    var n = 0;
    while (n<3 || n>9)
        n = prompt("How many sides?  \nBetween 3 and 9 sides please.", "3");
    var A = clickDownLocation;//shorter var names
    var B = clickUpLocation;
    var side = dist(A, B);//length of the side of the poly
    var Bprime = { x: A.x + side, y: A.y };//translated B to rectilinear
    var thetaPrime = Math.acos(((Math.pow(dist(Bprime, B), 2)) - (Math.pow(side, 2)) - (Math.pow(side, 2))) / (-2 * side * side));
    if (A.y > B.y)
        thetaPrime = -thetaPrime;
    var radius = side / (2 * Math.sin(Math.PI / n));//radius of the poly
    var height = side / (2 * Math.tan(Math.PI / n));//apothem of the poly(height of one the triangles)
    var C = { x: ((Bprime.x - A.x)/2) + A.x, y: A.y - height };//center point of the poly
    var Apolar = { r: radius, theta: ((Math.PI - (twoPI / n)) / 2 + Math.PI) };/*r,theta*///A converted to polar coordinates with C as center 0,0
    //calculate remaining points in polar
    //assume Apolar = p1
    //points are calculated and numberd clockwise from Apolar

    staticCtx.beginPath();
    staticCtx.moveTo(A.x, A.y);


    var pp2 = { r: radius, theta: Apolar.theta - (twoPI / n) };
    p2 = { x: pp2.r * Math.cos(-pp2.theta) + C.x, y: pp2.r * Math.sin(-pp2.theta) + C.y };
    p2 = translate(p2, A, thetaPrime);
    staticCtx.lineTo(p2.x, p2.y);

    //staticCtx.stroke();
    if (n >= 3) {
        var pp3 = { r: radius, theta: pp2.theta - (twoPI / n) };
        p3 = { x: pp3.r * Math.cos(-pp3.theta) + C.x, y: pp3.r * Math.sin(-pp3.theta) + C.y };
        p3 = translate(p3, A, thetaPrime);
        staticCtx.lineTo(p3.x, p3.y);
    }
    
    if (n >= 4) {
        var pp4 = { r: radius, theta: pp3.theta - (twoPI / n) };
        p4 = { x: pp4.r * Math.cos(-pp4.theta) + C.x, y: pp4.r * Math.sin(-pp4.theta) + C.y };
        p4 = translate(p4, A, thetaPrime);
        staticCtx.lineTo(p4.x, p4.y);
    }
    

    if (n >= 5) {
        var pp5 = { r: radius, theta: pp4.theta - (twoPI / n) };
        p5 = { x: pp5.r * Math.cos(-pp5.theta) + C.x, y: pp5.r * Math.sin(-pp5.theta) +C.y};
        p5 = translate(p5, A, thetaPrime);
        staticCtx.lineTo(p5.x, p5.y);
    }
    if (n >= 6) {
        var pp6 = { r: radius, theta: pp5.theta - (twoPI / n) };
        p6 = { x: pp6.r * Math.cos(-pp6.theta) + C.x, y: pp6.r * Math.sin(-pp6.theta)+C.y };
        p6 = translate(p6, A, thetaPrime);
        staticCtx.lineTo(p6.x, p6.y);
    }
    if (n >= 7) {
        var pp7 = { r: radius, theta: pp6.theta - (twoPI / n) };
        p7 = { x: pp7.r * Math.cos(-pp7.theta)+C.x, y: pp7.r * Math.sin(-pp7.theta)+C.y };
        p7 = translate(p7, A, thetaPrime);
        staticCtx.lineTo(p7.x, p7.y);
    }
    if (n >= 8) {
        var pp8 = { r: radius, theta: pp7.theta - (twoPI / n) };
        p8 = { x: pp8.r * Math.cos(-pp8.theta) + C.x, y: pp8.r * Math.sin(-pp8.theta) + C.y };
        p8 = translate(p8, A, thetaPrime);
        staticCtx.lineTo(p8.x, p8.y);
    }
    if (n >= 9) {
        var pp9 = { r: radius, theta: pp8.theta - (twoPI / n) };
        p9 = { x: pp9.r * Math.cos(-pp9.theta) + C.x, y: pp9.r * Math.sin(-pp9.theta) +C.y};
        p9 = translate(p9, A,  thetaPrime);
        staticCtx.lineTo(p9.x, p9.y);
    }

    switch (n) {
        case "3":
            pointArray = new Array(A, p2, p3);
            break;
        case "4":
            pointArray = new Array(A, p2, p3, p4);
            break;
        case "5":
            pointArray = new Array(A, p2, p3, p4, p5);
            break;
        case "6":
            pointArray = new Array(A, p2, p3, p4, p5, p6);
            break;
        case "7":
            pointArray = new Array(A, p2, p3, p4, p5, p6, p7);
            break;
        case "8":
            pointArray = new Array(A, p2, p3, p4, p5, p6, p7, p8);
            break;
        case "9":
            pointArray = new Array(A, p2, p3, p4, p5, p6, p7, p8, p9);
            break;
    }
    

    for (var i = 0; i < pointArray.length; i++) {
        if (pointArray[i].x > 500 || pointArray[i].x < 0 || pointArray[i].y > 500 || pointArray[i].y < 0)
            return;
    }


    staticCtx.closePath();
    staticCtx.stroke();
    drawPoint(A, black, staticCtx);
    drawPoint(p2, black, staticCtx);
    drawPoint(p3, black, staticCtx);
    if (n >= 4)
        drawPoint(p4, black, staticCtx);
    if (n >= 5)
        drawPoint(p5, black, staticCtx);
    if (n >= 6)
        drawPoint(p6, black, staticCtx);
    if (n >= 7)
        drawPoint(p7, black, staticCtx);
    if (n >= 8)
        drawPoint(p8, black, staticCtx);
    if (n >= 9)
        drawPoint(p9, black, staticCtx);
    //
    switch (n) {
        case "3":
            staticShapes.push(new Shape("triangle", 3, pointArray));
            break;
        case "4":
            staticShapes.push(new Shape("square", 4, pointArray));
            break;
        case "5":
            staticShapes.push(new Shape("pentagon", 5, pointArray));
            break;
        case "6":
            staticShapes.push(new Shape("hexagon", 6, pointArray));
            break;
        case "7":
            staticShapes.push(new Shape("heptagon", 7, pointArray));
            break;
        case "8":
            staticShapes.push(new Shape("octagon", 8, pointArray));
            break;
        case "9":
            staticShapes.push(new Shape("nonagon", 9, pointArray));
            break;
    }

}

function nudge(shape, dx, dy, ind, len) {
    var num = shape.n;
    for (var i = 0; i < num; i++) {
        if (shape.shape == "group") {
            nudge(shape.s[i], dx, dy);
        }
        else if (shape.shape == "circle") {
            radius = shape.p[1];
            newpoint = { x: shape.p[0].x + dx, y: shape.p[0].y + dy };
            dist1 = dist({ x: shape.p[0].x + dx, y: 0 }, newpoint);
            dist2 = dist({ x: shape.p[0].x + dx, y: 500 }, newpoint);
            dist3 = dist({ x: 0, y: shape.p[0].y + dy }, newpoint);
            dist4 = dist({ x: 500, y: shape.p[0].y + dy }, newpoint);
            if (radius >= dist1 || radius >= dist2 || radius >= dist3 || radius >= dist4) {
                onEdge = true;
                return;
            }
        } else {
            if (isOnEdge(shape.p[i].x + dx, shape.p[i].y) || isOnEdge(shape.p[i].x, shape.p[i].y + dy)) {
                onEdge = true;
                return;
            }
        }
    }
    if (shape.shape == "group" || shape.shape == "line")
        return;
    for (var i = 0; i < num; i++) {
        shape.p[i].x += dx;
        shape.p[i].y += dy;
    }
}

function checkNudge(shapeArr, dx, dy) {
    for (var i = 0; i < shapeArr.length; i++) {
        num = shapeArr[i].n;
        for (var j = 0; j < num; j++) {
            if (shapeArr[i].shape == "group") {
                for (j = 0; j < shapeArr[i].n; j++) {
                    if (!checkNudge(shapeArr[i].s, dx, dy))
                        return false;
                }
            }
            else if (shapeArr[i].shape == "circle") {
                radius = shapeArr[i].p[1];
                newpoint = { x: shapeArr[i].p[0].x + dx, y: shapeArr[i].p[0].y + dy };
                dist1 = dist({ x: shapeArr[i].p[0].x + dx, y: 0 }, newpoint);
                dist2 = dist({ x: shapeArr[i].p[0].x + dx, y: 500 }, newpoint);
                dist3 = dist({ x: 0, y: shapeArr[i].p[0].y + dy }, newpoint);
                dist4 = dist({ x: 500, y: shapeArr[i].p[0].y + dy }, newpoint);
                if (radius >= dist1 || radius >= dist2 || radius >= dist3 || radius >= dist4) {
                    return false;
                }
            } else {
                if (isOnEdge(shapeArr[i].p[j].x + dx, shapeArr[i].p[j].y) || isOnEdge(shapeArr[i].p[j].x, shapeArr[i].p[j].y + dy)) {
                    return false;
}
            }
        }
    }
    return true;
}

function isOnEdge(dx, dy) {
    if( dx >= 500 || dy >= 500 || dx <= 0 || dy <= 0 )
        return true;
    return false;
}

function nudgeSelection(shapeArr, dx, dy) {
    len = shapeArr.length;
    stillOnEdge = false;
    if (!onEdge) {
        okay = checkNudge(shapeArr, dx, dy);
        if (okay) {
            for (var i = 0; i < len; i++) {
                nudge(shapeArr[i], dx, dy, i, len);
            }
        }
        else
            return;
    } else {
        for (var i = 0; i < len; i++) {
            numPts = shapeArr[i].n;
            radius = shapeArr[i].p[1];
            newpoint = { x: shape.p[0].x + dx, y: shape.p[0].y + dy };
            if (shapeArr[i].shape == "circle") {
                dist1 = dist({ x: shapeArr[i].p[0].x, y: 0 }, newpoint);
                dist2 = dist({ x: shapeArr[i].p[0].x, y: 500 }, newpoint);
                dist3 = dist({ x: 0, y: shapeArr[i].p[0].y }, newpoint);
                dist4 = dist({ x: 500, y: shapeArr[i].p[0].y }, newpoint);
                if (radius >= dist1 || radius >= dist2 || radius >= dist3 || radius >= dist4) {
                    stillOnEdge = true;
                }
            } else {
                for (var j = 0; j < numPts; j++) {
                if (isOnEdge(shapeArr[i].p[j].x + dx, shapeArr[i].p[j].y + dy))
                    stillOnEdge = true;
            }
        }
        }
        if (!stillOnEdge) {
            onEdge = false;
            for (var i = 0; i < len; i++) {
                nudge(shapeArr[i], dx, dy)
            }
        }
    }
    
}

function drawText() {
    if (typing) {
        alert("Finish your previous text before starting another one!");
        return; // Still processing previous click
    }
    typingLocation = clickUpLocation;
    typing = true;
    textBuffer[0] = '_'; // _ is the cursor symbol
    dynctx.fillStyle = red;
    dynctx.textAlign = "left";
    dynctx.font = "10pt Geneva";
    dynctx.fillText('_', typingLocation.x, typingLocation.y);
}

function drawAllPoints(cnt, arr, clr) {
    for (var a = 0; a < arr.length; a++) {
        if (arr[a].shape == "group") {
            for (var b = 0; b < arr[a].n; b++) {
                if (arr[a].s[b].shape == "ellipse")
                    drawPoint(arr[a].s[b].p[0], clr, cnt);
                else if (arr[a].s[b].shape == "text")
                        ;
                else {
                    for (var c = 0; c < arr[a].s[b].n; c++) {
                        drawPoint(arr[a].s[b].p[c], clr, cnt);
                    }
                }
            }
        }
        else if (arr[a].shape == "ellipse") {
                drawPoint(arr[a].p[0], clr, cnt);
        }
        else if (arr[a].shape == "text")
            ;
        else {
            for (var b = 0; b < arr[a].n; b++) {
                drawPoint(arr[a].p[b], clr, cnt);
        }
    }
}
}

function singleSelect() {
    if (!shiftPressed) {
        staticShapes = staticShapes.concat(dynamicShapes);
        dynamicShapes.length = 0;
    }
    if (!(staticShapes.length > 0))
        return;
    var A = clickUpLocation;
    if( staticShapes[0].shape == "group" )
        var close = staticShapes[0].s[0].p[0];
    else
        var close = staticShapes[0].p[0];


    for (var a = 0; a < staticShapes.length; a++) {//group
        if (staticShapes[a].shape == "group") {
            for (var c = 0; c < staticShapes[a].n; c++) {//shape
                for( var d = 0; d < staticShapes[a].s[c].n; d++ ) {//point
                    if (dist(staticShapes[a].s[c].p[d], mouseLoc) < dist(close, mouseLoc)) {
                        close = staticShapes[a].s[c].p[d];
                        var selectedShape = a;
                    }
                }
            }
        }
        else {
            for (var b = 0; b < staticShapes[a].n; b++) {
                if (dist(staticShapes[a].p[b], mouseLoc) < dist(close, mouseLoc)) {
                    close = staticShapes[a].p[b];
                    var selectedShape = a;
                }
            }
        }
    }
    selectedShape = staticShapes.splice(selectedShape, 1);
    dynamicShapes.push(selectedShape[0]);
    restore(staticCtx, staticShapes, staticCanvas, black, true);
    restore(dynctx, dynamicShapes, dynCanvas, red, true);

}

function makeSelection() {
    if (!shiftPressed) {
        staticShapes = staticShapes.concat(dynamicShapes);
        dynamicShapes.length = 0;// clears the selected shapes
    }
    var shape;
    var A = clickDownLocation;
    var B = clickUpLocation;
    //look through every shape and add to selected array if all points are in selected field
    for (var a = 0; a < staticShapes.length; a++) {//for every shape
        if (within(staticShapes[a], A, B)) {//if a shape is in the selection box
            shape = staticShapes.splice(a, 1);
            dynamicShapes.push(shape[0]);
            a--;
        }
    }
    restore(staticCtx, staticShapes, staticCanvas, black, true);
    restore(dynctx, dynamicShapes, dynCanvas, red, true);

    setGroupRdio();
}

function setGroupRdio() {
    if (dynamicShapes.length == 1)
        group.selected = true;
    else
        ungroup.selected = true;
}

function vectorMove() {
    A = clickDownLocation;
    B = clickUpLocation;
    delx = B.x - A.x;
    dely = B.y - A.y;
    nudgeSelection(dynamicShapes, delx, dely);
    restore(dynctx, dynamicShapes, dynCanvas, "red", true);
}

function group() {
    var g = [];
    var  temp = [];
    copyArray(dynamicShapes, g);
    for (i = 0; i < g.length; i++) {
        if (g[i].shape == "group") {
            for (j = 0; j < g[i].n; j++) {
                temp.push(g[i].s[j]);
            }
        } else {
            temp.push(g[i]);
        }
    }
    dynamicShapes.push(new Group("group",temp.length,temp));
}

function ungroup() {
    var g =[];
    copyArray(dynamicShapes, g);
    for (var a = 0; a < g.length; a++) {// for every group
        switch (g[a].shape) {
            case "group":
                for (var b = 0; b < g[a].n;b++){//for every shape in the group
                    dynamicShapes.push(g[a].s[b]);
                }
                break;
            default:
                dynamicShapes.push(g[a]);
                break;
        }
    }
}

function copyArray(from, to) {
    var t;
    for (var a = 0; a < from.length; a++) {
        t = from.splice(a, 1);
        to.push(t[0]);
        a--;
    }
}
