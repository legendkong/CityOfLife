
const addSymbolButton = document.getElementById('add-symbol-button');
const symbolInput = document.getElementById('symbol-input');
const sharesInput = document.getElementById('shares-input');
const symbolList = document.getElementById('symbol-list');
const symbols = [];


// function showImage() {
//     image = document.createElement('image');
//     document.body.appendChild(image)
//     image.innerHTML = `<img src=https://logo.clearbit.com/${symbolInput.value}.com></img>`;
// }

// addSymbolButton.addEventListener('click', () => {
//    $('img[data-default-src]').each(function(){
//    var defaultSrc = $(this).data('default-src');
//    $(this).on('error', function(){
//      $(this).attr({src: defaultSrc}); 
//    });
// });
// });
    

//Fetch request from ESG API
addSymbolButton.addEventListener('click', () => {
    fetch(`https://tf689y3hbj.execute-api.us-east-1.amazonaws.com/prod/authorization/search?q=${symbolInput.value}&token=ddd9b621c5494b4af7b4d8d9312dc66b`).then((data)=>{
    return data.json();
}).then((completedata)=>{
    //console.log(completedata[0].environment_grade);
    document.getElementById('name'). innerHTML = completedata[0].company_name;

    //Environmental
    document.getElementById('env').innerHTML=
    "Grade: " + completedata[0].environment_grade + "<br>" + "Level: "+ completedata[0].environment_level + "<br>" + "Score: "+ completedata[0].environment_score;

    //Social
    document.getElementById('soc').innerHTML = "Grade: " + completedata[0].social_grade + "<br>" + "Level: "+ completedata[0].social_level + "<br>" + "Score: "+ completedata[0].social_score;

    //Governance
    document.getElementById('gov').innerHTML = "Grade: " + completedata[0].governance_grade + "<br>" + "Level: "+ completedata[0].governance_level + "<br>" + "Score: "+ completedata[0].governance_score;

}).catch((err)=>{
    console.log(err);
})});

//config is for chart.js, initialised type as doughnut chart
const config = {
    type: 'doughnut',
    data: {
        labels: [],
        datasets: [{
            label: 'My Portfolio',
            backgroundColor: [],
            data: [],
        }]
    },
    options: {}
};

const myChart = new Chart(
    document.getElementById('chart'),
    config
);

//Event listener for add symbol button which activates backend request and displays data
addSymbolButton.addEventListener('click', () => {
    const symbolInputValue = symbolInput.value.toUpperCase();
    const sharesInputValue = +sharesInput.value;
    addSymbol(symbolInputValue, sharesInputValue);
    symbolInput.value = "";
    sharesInput.value = "";
});

function addSymbol(symbol, shares) {
    fetch("/price?symbol=" + symbol)
        .then(response => response.json())
        .then(data => {
            const symbolData = {...data, shares};
            symbols.push(symbolData);
            drawList();
            addSymbolToChart(symbolData);
        });
}

function drawList() {
    symbolList.innerHTML = "";
    symbols.forEach((symbol) => {
        console.log(symbol);
        const li = document.createElement('li');
        li.innerText = symbol.shares +" "+ symbol.symbol + " " + " x " + "$" + symbol.price + " = " + "$" + round(symbol.price * symbol.shares);
        symbolList.appendChild(li);
    });
}

function addSymbolToChart(symbol) {
    myChart.data.labels.push(symbol.symbol);
    myChart.data.datasets[0].data.push(round(symbol.shares * symbol.price));
    myChart.data.datasets[0].backgroundColor.push(getRandomColor());
    myChart.update();
}

//Function to generate random colour for the chart
function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

//Function to round final price
function round(value) {
    return Math.round(value * 100) / 100;
}