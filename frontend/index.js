
const addSymbolButton = document.getElementById('add-symbol-button');
const symbolInput = document.getElementById('symbol-input');
const sharesInput = document.getElementById('shares-input');
const symbolList = document.getElementById('symbol-list');
const symbols = [];
const openE = document.getElementById('openE');
const openS = document.getElementById('openS');
const openG = document.getElementById('openG');
const openOverall = document.getElementById('openOverall');
const modal_containerE = document.getElementById('modal_containerE');
const modal_containerS = document.getElementById('modal_containerS');
const modal_containerG = document.getElementById('modal_containerG');
const modal_containerOverall = document.getElementById('modal_containerOverall');
const closeE = document.getElementById('closeE');
const closeS = document.getElementById('closeS');
const closeG = document.getElementById('closeG');
const closeOverall = document.getElementById('closeOverall');
var environment = [];
var social = [];
var governance = [];
var totalValueOfShares = [];




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

    document.getElementById('name'). innerHTML = completedata[0].company_name;

    //Environmental
    document.getElementById('env').innerHTML=
    "Grade: " + completedata[0].environment_grade + "<br>" + "Level: "+ completedata[0].environment_level + "<br>" + "Score: "+ completedata[0].environment_score;

    //Social
    document.getElementById('soc').innerHTML = "Grade: " + completedata[0].social_grade + "<br>" + "Level: "+ completedata[0].social_level + "<br>" + "Score: "+ completedata[0].social_score;

    //Governance
    document.getElementById('gov').innerHTML = "Grade: " + completedata[0].governance_grade + "<br>" + "Level: "+ completedata[0].governance_level + "<br>" + "Score: "+ completedata[0].governance_score;

    //Initializing sum of E,S,G variables
    let sumEnv = 0;
    let sumSoc = 0;
    let sumGov = 0;

    //Storing [i] Environmental score into an array
    environment.push(completedata[0].environment_score);
    for(let i = 0 ; i < environment.length; i++){      
        environment[i] = environment[i] * portfolioPercentage[i]; //<-- implemented
    //sumEnv += environment[i];
    }  

    //To get average Environment score
    //sumEnv = sumEnv/(environment.length); 
    //sumEnv = Math.round(sumEnv * 10) / 10;
    sumEnv += environment[i]; //<-- implemented
    document.getElementById('totalEnv').innerHTML = "Total Environmental Score : " + sumEnv;

    //Storing [i] Social score into an array
    social.push(completedata[0].social_score);
    for(let i = 0 ; i < social.length; i++){
    sumSoc += social[i];
    }  
    //To get average Social score
    sumSoc = sumSoc/(social.length);
    sumSoc = Math.round(sumSoc * 10) / 10;
    document.getElementById('totalSoc').innerHTML = "Total Social Score : " + sumSoc;

    //Storing [i] Governance score into an array
    governance.push(completedata[0].governance_score);
    for(let i = 0 ; i < governance.length; i++){
    sumGov += governance[i];
    }  
    //To get average Governance score
    sumGov = sumGov/(governance.length);
    sumGov = Math.round(sumGov * 10) / 10;
    document.getElementById('totalGov').innerHTML = "Total Governance Score : " + sumGov;
    
    


//write function to download text file
function download(filename, text) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}
//download text file
download("ESG.txt", ("Total Environment score: " + sumEnv + "\n" + "Totol Social Score: " + sumSoc + "\n" + "Total Governance Score: " + sumGov));

    //To get healthbar %
    totalSum = sumEnv + sumSoc + sumGov;
    totalSum = totalSum/10;
    totalSum = Math.round(totalSum * 10) / 10;

    //Creating healthbar
    class HealthBar {
    constructor (element, initialValue = 0) {
        this.valueElem = element.querySelector('.health-bar-value');
        this.fillElem = element.querySelector('.health-bar-fill');
        this.setValue(initialValue);
    }

    setValue(newValue){
        //Min 0%
        if(newValue < 0) {
            newValue = 0;
        }
        //Max 100%
        if(newValue > 100){
            newValue = 100;
        }
        this.value = newValue;
        this.update();
    }

    update(){
        const percentage = this.value + '%'; 
        this.fillElem.style.width = percentage;
        this.valueElem.textContent = percentage;
    }
}
//Healthbar object
const hb = new HealthBar(document.querySelector('.health-bar'), totalSum);
hb.setValue(totalSum);
    
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
openOverall.addEventListener('click', () => {
    modal_containerOverall.classList.add('show');
});

//Modal close button
closeE.addEventListener('click', () => {
    modal_containerE.classList.remove('show');
});
closeS.addEventListener('click', () => {
    modal_containerS.classList.remove('show');
});
closeG.addEventListener('click', () => {
    modal_containerG.classList.remove('show');
});
closeOverall.addEventListener('click', () => {
    modal_containerOverall.classList.remove('show');
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

            //Array of stock holdings value stored
            totalValueOfShares.push(totalStockPrice);
            console.log(totalValueOfShares);
            
            //Total portfolio value (sum of all stock holdings)
            let totalPortfolioValue = 0;
            for (let i = 0; i < totalValueOfShares.length; i++) {
                totalPortfolioValue = totalPortfolioValue + totalValueOfShares[i];
            }
            document.getElementById("totalPortfolioValue").innerHTML = "Total holdings: " + Math.round(totalPortfolioValue);
            console.log(totalPortfolioValue);

            //Stock percentage contribution array
            portfolioPercentage = [];
            for(let i = 0; i < totalValueOfShares.length; i++) {
                percentage = totalValueOfShares[i] / totalPortfolioValue;
                portfolioPercentage.push(percentage);
            }
            console.log(portfolioPercentage);

            //Add symbol to chart 
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
        totalStockPrice = round(symbol.price * symbol.shares);
        
    });
}



function addSymbolToChart(symbol) {
    myChart.data.labels.push(symbol.symbol);
    myChart.data.datasets[0].data.push(round(symbol.shares * symbol.price));
    myChart.data.datasets[0].backgroundColor.push(getRandomColor());
    myChart.update();
}


function sellStockToChart(symbol) {
    myChart.data.datasets[0].data[symbol] = 50; // Would update the first dataset's value of 'March' to be 50
    myChart.update(); // Calling update now animates the position of March from 90 to 50.
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

