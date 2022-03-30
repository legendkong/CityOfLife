
const addSymbolButton = document.getElementById('add-symbol-button');
const symbolInput = document.getElementById('symbol-input');
const sharesInput = document.getElementById('shares-input');
const symbolList = document.getElementById('symbol-list');
const symbols = [];
const openE = document.getElementById('openE');
const openS = document.getElementById('openS');
const openG = document.getElementById('openG');
const modal_containerE = document.getElementById('modal_containerE');
const modal_containerS = document.getElementById('modal_containerS');
const modal_containerG = document.getElementById('modal_containerG');
const closeE = document.getElementById('closeE');
const closeS = document.getElementById('closeS');
const closeG = document.getElementById('closeG');
var environment = [];
var social = [];
var governance = [];


// function showImage() {
//     image = document.createElement('image');
//     document.body.appendChild(image)
//     image.innerHTML = `<img src=https://logo.clearbit.com/${symbolInput.value}.com></img>`;
// }

// var img = document.createElement("img");
// img.src = `https://logo.clearbit.com/${symbolInput.value}.com`;
// var src = document.getElementById("image");
// src.appendChild(img);

function showDiv() {
    document.getElementById('bodypf').style.display = "block";
}


addSymbolButton.addEventListener('click', () =>  {
    var img = document.createElement("img");
    img.src = `https://logo.clearbit.com/${symbolInput.value}.com`;
    var src = document.getElementById("image");
    src.appendChild(img); //Adds one element to the node

    //If don't want images to stack:
    document.getElementById("image").innerHTML = ""; // Clears the image div, so that the images wont stack 
    document.getElementById("image").appendChild(img);
});









//Fetch request from SDG data API
addSymbolButton.addEventListener('click', () => {
    fetch(`https://tf689y3hbj.execute-api.us-east-1.amazonaws.com/prod/authorization/goals?q=${symbolInput.value}&token=ddd9b621c5494b4af7b4d8d9312dc66b`).then((data)=>{
    return data.json();
}).then((completedata)=>{

    //SDG goals
    document.getElementById('goals').innerHTML=
    "Goal 1: " + completedata[0].goals[0].sdg + "<br>" + "Goal 2: "+ completedata[0].goals[1].sdg + "<br>" + "Goal 3: "+ completedata[0].goals[2].sdg + "<br>" + "Goal 4: "+ completedata[0].goals[3].sdg + "<br>" + "Goal 5: "+ completedata[0].goals[4].sdg;

}).catch((err)=>{
    console.log(err);
})});



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

    //Storing [i] Environmental score into an array
    environment.push(completedata[0].environment_score);
    console.log('Environment', environment)

    //Storing [i] Social score into an array
    social.push(completedata[0].social_score);
    console.log('Social', social)

    //Storing [i] Governance score into an array
    governance.push(completedata[0].governance_score);
    console.log('Governance', governance)



}).catch((err)=>{
    console.log(err);
})});


//Show modal pop-up
openE.addEventListener('click', () => {
    modal_containerE.classList.add('show');
});
openS.addEventListener('click', () => {
    modal_containerS.classList.add('show');
});
openG.addEventListener('click', () => {
    modal_containerG.classList.add('show');
});

// Modal close button
closeE.addEventListener('click', () => {
    modal_containerE.classList.remove('show');
});
closeS.addEventListener('click', () => {
    modal_containerS.classList.remove('show');
});
closeG.addEventListener('click', () => {
    modal_containerG.classList.remove('show');
});


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