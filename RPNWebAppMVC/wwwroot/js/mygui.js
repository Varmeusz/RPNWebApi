let FizzyText = function() {
    this.scale = 10;
    this.shiftX = 0;
    this.shiftY = 0;
};
function Update() {
	text = new FizzyText();
    var gui = new dat.GUI();
    gui.domElement.id = 'gui';
	let scaleControl = gui.add(text, 'scale', 1.0, 100);
	let shiftXControl = gui.add(text, 'shiftX', -1000, 1000);
    let shiftYControl = gui.add(text, 'shiftY', -1000, 1000);
    
    scaleControl.onChange(function(value) {
        scale = value;
        redrawCanvas();
        if(!errors)
        rePlot(curData);
        else 
        rePlotHoley(curData);
        
    });
    shiftXControl.onChange(function(value) {
        shiftX = value;
        redrawCanvas();
        if(!errors)
        rePlot(curData);
        else 
        rePlotHoley(curData);        
    });
    shiftYControl.onChange(function(value) {
        shiftY = value;
        redrawCanvas();
        if(!errors)
        rePlot(curData);
        else 
        rePlotHoley(curData);        
    });
}