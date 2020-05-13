let canvas;
let ctx;
let maxX, maxY;
function initCanvas(){
    canvas = document.getElementById('canvasso');
    ctx  = canvas.getContext('2d');
    maxX = canvas.width;
    maxY = canvas.height;    
    ctx.beginPath();
    ctx.moveTo(0,maxY/2);
    ctx.lineTo(maxX, maxY/2);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(maxX/2, 0);
    ctx.lineTo(maxX / 2, maxY);
    ctx.stroke();
}
function redrawCanvas(){
    console.log(ctx);

    ctx.clearRect(0,0,maxX,maxY);
    ctx.beginPath();
    ctx.moveTo(0,maxY/2);
    ctx.lineTo(maxX, maxY/2);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(maxX/2, 0);
    ctx.lineTo(maxX / 2, maxY);
    ctx.stroke();
}
