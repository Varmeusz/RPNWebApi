
		var randomScalingFactor = function() {
			return Math.round(Math.random() * 100);
		};
window.chartColors = {
	red: 'rgb(255, 99, 132)',
	orange: 'rgb(255, 159, 64)',
	yellow: 'rgb(255, 205, 86)',
	green: 'rgb(75, 192, 192)',
	blue: 'rgb(54, 162, 235)',
	purple: 'rgb(153, 102, 255)',
	grey: 'rgb(201, 203, 207)',
	black: 'rgp(0,0,0)'
};
		var config = {
			type: 'pie',
			data: {
				datasets: [{
					data: [
						0.75,
						0.25
					],
					backgroundColor: [
						window.chartColors.yellow,
						window.chartColors.black
					],
					label: 'Dataset 1'
				}],
				labels: [
					'Pacman',
					
				]
			},
			options: {
				title: {
					display: true,
					text: 'Wykres stworzony przy pomocy chart.js przedstawiający Pacmana',
					position: 'bottom'
				},
				rotation: (Math.PI/4),
				animation:false,
				responsive: true,
				maintainAspectRatio:false
			}
		};

		window.onload = function() {
			var ctx = document.getElementById('myChart').getContext('2d');
			window.myPie = new Chart(ctx, config);
		};

let flag=true;
function eat(){
	if(flag){
		window.myPie.options.rotation=(Math.PI/4)-0.75;
    window.myPie.data.datasets[0].data.pop();
		window.myPie.data.datasets[0].data[0]=1.0;
    window.myPie.update();
		flag = false;
		
	}else{window.myPie.options.rotation=Math.PI/4;
		
		window.myPie.data.datasets[0].data[0]=0.75;
    window.myPie.data.datasets[0].data.push(0.25);
    window.myPie.update();
		flag = true;
	}
	
}
let intervalFlag = true;
let varinterval;
function controlInterval(){
	if(intervalFlag){
		varinterval = setInterval(eat,500);
		intervalFlag=false;
	}else{
		clearInterval(varinterval);
		intervalFlag=true;
	}
	
}