
//startup functions only.  These functions tend to only run once at page load 

//Draws Grid on staticCanvas
function drawGrid() {
    var i;
    gridCtx.clearRect(0, 0, 500, 500);
    gridCtx.beginPath();
    for (i = 20; i < 500; i += 20) {
        gridCtx.moveTo(i, 0);
        gridCtx.lineTo(i, 500);
        gridCtx.moveTo(0, i);
        gridCtx.lineTo(500, i);
    }
    gridCtx.closePath();
    gridCtx.strokeStyle = "grey";
    gridCtx.stroke();
}

//initializes the staticCanvas on the page
function start_canvas() {
    staticCanvas = document.getElementById('static');
    topCanvas = document.getElementById('top');
    dynCanvas = document.getElementById('dynamic');
    gridCanvas = document.getElementById('grid');

    if (gridCanvas.getContext) {
        gridCtx = gridCanvas.getContext('2d');
        //staticCanvas.onmousedown = mousedown;
        //staticCanvas.onmouseup = mouseup;
        // staticCanvas.onmousemove = mouseMove;  //can bring back if we need it
        //staticCanvas.onkeypress = keypress;
        // Make sure than NW corner of staticCanvas is screen coordinates (0,0)
        for (var o = gridCanvas; o; o = o.offsetParent) {
            offset_left3 += (o.offsetLeft - o.scrollLeft);
            offset_top3 += (o.offsetTop - o.scrollTop);
        }
    }

    //bottom staticCanvas, for shapes and lines that have been drawn
    if (staticCanvas.getContext) {
        staticCtx = staticCanvas.getContext('2d');
        //staticCanvas.onmousedown = mousedown;
        //staticCanvas.onmouseup = mouseup;
        // staticCanvas.onmousemove = mouseMove;  //can bring back if we need it
        //staticCanvas.onkeypress = keypress;
        // Make sure than NW corner of staticCanvas is screen coordinates (0,0)
        for (var o = staticCanvas; o; o = o.offsetParent) {
            offset_left += (o.offsetLeft - o.scrollLeft);
            offset_top += (o.offsetTop - o.scrollTop);
        }
    }
    //middle dynamic canvas, for text writing and selected objects
    if (dynCanvas.getContext) {
        dynctx = dynCanvas.getContext('2d');
        dynctx.strokeStyle = "red";
        dynctx.fillStyle = "red";
        //staticCanvas.onmousedown = mousedown;
        //staticCanvas.onmouseup = mouseup;
        // staticCanvas.onmousemove = mouseMove;  //can bring back if we need it
        //staticCanvas.onkeypress = keypress;
        // Make sure than NW corner of staticCanvas is screen coordinates (0,0)
        for (var o = dynCanvas; o; o = o.offsetParent) {
            offset_left2 += (o.offsetLeft2 - o.scrollLeft2);
            offset_top2 += (o.offsetTop2 - o.scrollTop2);
        }
    }

    //top canvas, for red line and event handlers
    if (topCanvas.getContext) {
        topctx = topCanvas.getContext('2d');
        topCanvas.onmousedown = mousedown;
        topCanvas.onmouseup = mouseup;
        topCanvas.onmousemove = mouseMove;  //can bring back if we need it
        topCanvas.onkeydown = keyhandle;
        document.body.onkeydown = arrowhandle;
        document.body.onkeyup = shiftOff;

        // Make sure than NW corner of staticCanvas is screen coordinates (0,0)
        for (var o = topCanvas; o; o = o.offsetParent) {
            offset_left1 += (o.offsetLeft - o.scrollLeft);
            offset_top1 += (o.offsetTop - o.scrollTop);
        }
    }

    drawGrid();
};
//end startup functions###############################
