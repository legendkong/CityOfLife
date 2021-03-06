# City of Life:family_man_woman_girl_boy::cityscape::city_sunset:
<p align="center">
A gamification solution for ESG stock portfolio ratings. <br> <br>
<img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/OverallGood.gif" width="600"> <br>
</p>

## Use Cases
Users are able to port over their existing stock portfolio from their brokerage into City of Life. The user's investments will be uniquely digitalized within the application.
<br><br>
When the user purchases shares of a company, the app will automatically convert the ESG ratings of said company into varying conditions in the city. Events (e.g air pollution) might also occur from time to time throughout the city, depending on the nature and ESG sub-ratings of the company.

## Summary
Data visualization of the ESG ratings of your portfolio through a simulation. ESG ratings of your portfolio, instead of being represented through mere numbers and graphs, are represented through a real-world city simulation.

## Application code
***Front-End*** <br>
Runtime environment: [NodeJS](https://nodejs.org/en/) <br>
Build tool: npm <br>
APIs: <br>
[ESG scores and ratings API](https://www.esgenterprise.com/esg-enterprise-data-api-services/) <br>
[Company logo API](https://clearbit.com/blog/logo) <br>
[Yahoo Finance historical quotes and snapshot data downloader](https://www.npmjs.com/package/yahoo-finance)<br>
External links: <br>
[Goldman Sachs ESG report](https://www.goldmansachs.com/investor-relations/corporate-governance/sustainability-reporting/)<br>
[United Nations SDG](https://sdgs.un.org/goals)<br>

***Back-End*** <br>
Development platform : [Unity](https://unity.com/)<br>

# Front-End
A stock portfolio app that allows user to buy and sell stocks, then to retrieve and feed overall weighted E, S and G data to the back-end, based on the user's portfolio.
<p align="center">
  <img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/FinalFrontEnd.gif" width="400"> <br>
  Features of the app.
</p>

## Features
<p align="center">
<img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/EnvView.PNG" width="300"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/SocView.PNG" width="300"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/govView.PNG" width="300"> <br>
Educational informations to boost user's knowledge and awareness on ESG.
  <br>
  <br>
  <br>
  <br>
  <img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/ESGreport.PNG" width="600"><br>
  Review and recommendations based on user portfolio's overall ESG scores.
</p>
<br>

# Connecting the Front-End to Back-End
Whenever the user makes changes to his portfolio - i.e., to buy or sell stocks, the overall E, S and G scores will change according to the portfolio allocation and the E, S and G scores of the individual stock(s) he/she owns. <br>

For example, if user owns _**1 share of TSLA ($1000)**_ and _**10 shares of NIO ($20 x 10 = $200)**_,
```
Total portfolio allocation = 83% TSLA, 17% NIO (Total sum $1200) 

<TSLA> E, S, G scores = 500, 250, 300 
<NIO> E, S, G scores = 400, 300, 350 

Weighted <TSLA> E, S, G scores = (500*83%, 250*83%, 300*83%) ---> 415, 207.5, 249
Weighted <NIO> E, S, G scores = (400*17%, 300*17%, 350*17%) ---> 68, 51, 59.5 

Overall E, S, G scores** = (415+68, 207.5+51, 249+59.5) ---> 483, 258.5, 308.5
```
After the overall E, S and G scores are updated, the data will be saved, downloaded and fed into our back-end application where it will read the values and modify the state of the city.
<br><br>

# Back-End
The state of the city changes accordingly and proportionately to the E, S and G scores being fed from the front-end to the back-end. <br>
Below are some scenarios when the E, S and G score changes.

## Good vs Bad Environmental (E) score

<table align="center">
  <tr align="center">
   <p align="center"> 
    Oxygen (Trees) 
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodEmoreTrees.gif" width="400" height="200"/><br> More trees will appear as the E value increases.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/BadElesstrees.gif" width="400" height="200"/><br> More trees will be chopped down as the E value decreases.</td>
  </tr>
</table>
<br>

<table align="center">
  <tr align="center">
    <p align="center"> 
    Energy (Windmills)
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodEwindmill.gif" width="400" height="200"/><br> More windmills will be planted as the E value increases.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/BadEwindmill.gif" width="400" height="200"/><br>  Windmills will start to get damaged as the E value decreases.</td>
  </tr>
</table>
<br>

<table align="center">
  <tr align="center">
    <p align="center"> 
    Air pollution (Factories)
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodElesserfactories.gif" width="400" height="200"/><br> Air will not be as polluted as E value increases<br>(Lesser factories built)</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/BadEfactories.gif" width="400" height="200"/><br> Air will start to get polluted as E value decreases<br>(More factories built)</td>
  </tr>
</table>
<br>

<table align="center">
  <tr align="center">
    <p align="center"> 
    Water pollution (Rivers)
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/goodWater.PNG" width="400" height="200"/><br> Rivers will be cleaner when the E score is good.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/notSoGoodWater.PNG" width="400" height="200"/><br> Rivers will start to turn green as the E score decreases.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/badWater.PNG" width="400" height="200"/><br> Yucks! </td>
  </tr>
</table>
<br><br><br>

## Good vs Bad Social (S) score

<table align="center">
  <tr align="center">
   <p align="center"> 
    Standard of Living (Houses) 
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodSmoreHouse.PNG" width="400" height="200"/><br> More occupied houses will appear as the S score increases. <br> This signifies that more people wants to live in the city.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/BadSlesserHouses.PNG" width="400" height="200"/><br> Houses will start to be vacant as the S score decreases.</td>
  </tr>
</table>
<br>

<table align="center">
  <tr align="center">
    <p align="center"> 
    Happiness (People)
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodSthrivingeconomy.gif" width="400" height="200"/><br> Happier citizens will roam the city as the S score increases.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/BadSfustratedpeople.gif" width="400" height="200"/><br> Citizens start turning frustrated as the S score decreases. </td>
  </tr>
</table>
<br><br><br>

## Good vs Bad Governance (G) score

<table align="center">
  <tr align="center">
    <p align="center"> 
    Thriving economy (Companies)
    </p>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/GoodSmorebuildings.PNG" width="400" height="200"/><br> More companies will move in as the G score increases.</td>
    <td valign="center"><img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/badSlesserBuildings.PNG" width="400" height="200"/><br> Demand for building plots will drop as the G score decreases. </td>
  </tr>
</table>
<br><br><br>

# Overview
<p align="center">
  <img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/OverallGood.gif" width="600"> <br>
  How a city with good ESG score looks.
</p>
<br><br>
<p align="center">
  <img src="https://github.com/legendkong/CityOfLife/blob/master/CoLmarkdown/OverallBad.gif" width="600"> <br>
  And in every seed of good, there is always a piece of bad. 
</p>

## Footnotes
What we are trying to achieve by building **City of Life** is to allow ESG to be more understandable, digestible and useable for the average investors, and more so for the new investors who have never heard of ESG investing. <br><br>
By implementing and building a gamified solution, we were able to translate "data" into visually impactful real-world-city scenarios where instead of just numbers and figures, users will be able to see and learn the benefits of ESG investing and also the disadvantages of not considering ESG while investing. <br>

_**Our world is changing faster than anyone predicted. We believe responsible investing has a huge role to play in shaping a better world and building a sustainable future.**_
