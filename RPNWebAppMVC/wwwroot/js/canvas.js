let canvas;
let ctx;
let maxX, maxY;
function initCanvas(){
    canvas = document.getElementById('canvasso');
    ctx  = canvas.getContext('2d');
    ctx.imageSmoothingEnabled = true;
    maxX = canvas.width;
    maxY = canvas.height;   
    ctx.lineWidth=2; 
    ctx.beginPath();
    ctx.moveTo(0,maxY/2);
    ctx.lineTo(maxX, maxY/2);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(maxX/2, 0);
    ctx.lineTo(maxX / 2, maxY);
    ctx.stroke();
    ctx.lineWidth = 0.5;
    ctx.lineCap = 'round';
}
function redrawCanvas(){
    //console.log(ctx);
    ctx.lineWidth=2;
    ctx.clearRect(0,0,maxX,maxY);
    ctx.beginPath();
    ctx.moveTo(0+shiftX,maxY/2 + shiftY);
    ctx.lineTo(maxX + shiftX, maxY/2 + shiftY);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(maxX/2 + shiftX, 0+shiftY);
    ctx.lineTo(maxX / 2 + shiftX, maxY + shiftY);
    ctx.stroke();
    ctx.lineWidth=0.5;
}
function rePlot(data){
    ctx.beginPath();
    for(let i = 0; i < data.result.length; i++){
        //let scale = 10;
        if(i==0)
        {
            ctx.moveTo(maxX/2 + data.result[i].x*scale + shiftX, maxY/2 - data.result[i].y*scale + shiftY);
            continue;
        }
        ctx.lineTo(maxX/2 + data.result[i].x*scale + shiftX, maxY/2 - data.result[i].y * scale + shiftY);
    }
    ctx.stroke();
}

function rePlotHoley(data){
    ctx.beginPath();
    for(let i = 0; i < data.result.length; i++){
        if(data.result[i].error == null){
            if(i==0)
            {
                ctx.moveTo(maxX/2 + data.result[i].x*scale + shiftX, maxY/2 - data.result[i].y*scale + shiftY);
                continue;
            }
            ctx.lineTo(maxX/2 + data.result[i].x*scale + shiftX, maxY/2 - data.result[i].y * scale + shiftY);
        }else{
            ctx.stroke();
            ctx.beginPath();
        }
        
    }
    ctx.stroke();
}