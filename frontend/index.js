
var buySymbolButton = document.getElementById('buy-symbol-button');
var symbolInput = document.getElementById('symbol-input');
var sharesInput = document.getElementById('shares-input');
var symbolList = document.getElementById('symbol-list');
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
const symbols = [];
var environment = [];
var social = [];
var governance = [];
var totalValueOfShares = [];

/*-----------------------Fetch request to get company logo image--------------------------*/
buySymbolButton.addEventListener('click', () =>  {
    var img = document.createElement("img");
    img.src = `https://logo.clearbit.com/${symbolInput.value}.com`;
    var src = document.getElementById("image");
    src.appendChild(img); //Adds one element to the node
    //If don't want images to stack:
    document.getElementById("image").innerHTML = ""; // Clears the image div, so that the images wont stack 
    document.getElementById("image").appendChild(img);
});


/*-----------------------Fetch request to get Sustainable Development Goals(UN) data --------------------------*/
buySymbolButton.addEventListener('click', () => {
    fetch(`https://tf689y3hbj.execute-api.us-east-1.amazonaws.com/prod/authorization/goals?q=${symbolInput.value}&token=ddd9b621c5494b4af7b4d8d9312dc66b`).then((data)=>{
    return data.json();
}).then((completedata)=>{
    //SDG goals
    document.getElementById('goals').innerHTML=
    "Goal 1: " + completedata[0].goals[0].sdg + "<br>" + "Goal 2: "+ completedata[0].goals[1].sdg + "<br>" + "Goal 3: "+ completedata[0].goals[2].sdg + "<br>" + "Goal 4: "+ completedata[0].goals[3].sdg + "<br>" + "Goal 5: "+ completedata[0].goals[4].sdg;
}).catch((err)=>{
    console.log(err);
})});

/*----------------------------------------Fetch request from ESG data API-----------------------------------------*/
buySymbolButton.addEventListener('click', () => {
    fetch(`https://tf689y3hbj.execute-api.us-east-1.amazonaws.com/prod/authorization/search?q=${symbolInput.value}&token=ddd9b621c5494b4af7b4d8d9312dc66b`).then((data)=>{
    return data.json();
}).then((completedata)=>{
    //Display company name
    document.getElementById('name'). innerHTML = completedata[0].company_name;

    //Display Environmental scores
    document.getElementById('env').innerHTML=
    "Grade: " + completedata[0].environment_grade + "<br>" + "Level: "+ completedata[0].environment_level + "<br>" + "Score: "+ completedata[0].environment_score;

    //Display Social scores
    document.getElementById('soc').innerHTML = "Grade: " + completedata[0].social_grade + "<br>" + "Level: "+ completedata[0].social_level + "<br>" + "Score: "+ completedata[0].social_score;

    //Display Governance scores
    document.getElementById('gov').innerHTML = "Grade: " + completedata[0].governance_grade + "<br>" + "Level: "+ completedata[0].governance_level + "<br>" + "Score: "+ completedata[0].governance_score;

    /*-------------------CALCULATION OF WEIGHTED E, S, G ratings according to portfolio allocation %-------------------*/
    //Initializing sum of E,S,G variables
    let sumEnv = 0;
    let sumSoc = 0;
    let sumGov = 0;

    //Storing [i] Environmental score into an array
    environment.push(completedata[0].environment_score);
    var weightedE = [];
    for(let i = 0 ; i < environment.length; i++){      
        //weightedE array = individual env score * holdings percentage
        weightedE[i] = environment[i] * portfolioPercentage[i];
        //sum of weightedE array elements
        sumEnv += weightedE[i]; 
    }  
    //Display total weighted Environment score
    document.getElementById('totalEnv').innerHTML = "Total Environmental Score : " + Math.round(sumEnv);

    //Storing [i] Social score into an array
    social.push(completedata[0].social_score);
    var weightedS = [];
    for(let i = 0 ; i < social.length; i++){
        //weightedS array = individual soc score * holdings percentage
        weightedS[i] = social[i] * portfolioPercentage[i];
        //sum of weightedS array elements
        sumSoc += weightedS[i];
    }  
    //Display total weighted Social score
    document.getElementById('totalSoc').innerHTML = "Total Social Score : " + Math.round(sumSoc);

    //Storing [i] Governance score into an array
    governance.push(completedata[0].governance_score);
    var weightedG = [];
    for(let i = 0 ; i < governance.length; i++){
        //weightedG array = individual gov score * holdings percentage
        weightedG[i] = governance[i] * portfolioPercentage[i];
        //sum of weightedS array elements
        sumGov += weightedG[i];
    }  
    //Display total weighted Governance score
    document.getElementById('totalGov').innerHTML = "Total Governance Score : " + Math.round(sumGov);
    
    
    /*-------------------Function to write and save data(weighed E,S,G values) into text file-------------------*/
    function download(filename, text) {
        var element = document.createElement('a');
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
        element.setAttribute('download', filename);
        element.style.display = 'none';
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    }
    //Text file auto download
    download("CityOfLife_ESG.txt", ("m_E: " + sumEnv + "\n" + "m_S: " + sumSoc + "\n" + "m_G: " + sumGov));

    /*---------------------------HEALTHBAR FEATURE------------------------------*/
    //To get healthbar %
    totalSum = sumEnv + sumSoc + sumGov - 100;
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

/*------------------------------------MODAL FEATURES--------------------------------------- */
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


/*------------------------------------INITIALIZE CHARTJS--------------------------------------- */
//config for chart.js, initialised type as doughnut chart
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
buySymbolButton.addEventListener('click', () => {
    const symbolInputValue = symbolInput.value.toUpperCase();
    const sharesInputValue = +sharesInput.value;
    addSymbol(symbolInputValue, sharesInputValue);
    symbolInput.value = "";
    sharesInput.value = "";
});

/*------------------------------------ON CLICK ADD SYMBOL--------------------------------------- */
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
            document.getElementById("totalPortfolioValue").innerHTML = "Total holdings: $ " + Math.round(totalPortfolioValue);
            console.log(totalPortfolioValue);

            //Stock percentage contribution array
            portfolioPercentage = [];
            for(let i = 0; i < totalValueOfShares.length; i++) {
                percentage = totalValueOfShares[i] / totalPortfolioValue;
                portfolioPercentage.push(percentage);
            }
            console.log(portfolioPercentage);
            
            if(symbolData.shares > 0){
            //Add symbol to chart 
            addSymbolToChart(symbolData);         
            }else if(symbolData.shares < 0){
            minusSymbolFromChart(symbolData)
            };
        });
}

/*------------------------------------DISPLAY OF STOCK HOLDINGS--------------------------------------- */
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


/*------------------------------------UPDATE CHART FEATURE--------------------------------------- */
function addSymbolToChart(symbol) {
    myChart.data.labels.push(symbol.symbol);
    myChart.data.datasets[0].data.push(round(symbol.shares * symbol.price));
    myChart.data.datasets[0].backgroundColor.push(getRandomColor());
    myChart.update(); 
}
function minusSymbolFromChart(symbol) {
    myChart.reset();
    myChart.data.datasets[0].data.pop(round(symbol.shares * symbol.price));
    myChart.data.datasets[0].backgroundColor.push(getRandomColor());
    myChart.update(); 
}


/*------------------------------------MISCELLANEOUS FUNCTIONS --------------------------------------- */
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

//Show Div upon button click
function showDiv() {
    document.getElementById('bodypf').style.display = "block";
}
