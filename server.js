const yahooFinance = require('yahoo-finance');
const express = require('express');
const app = express();
const port = 3000;

app.use(express.static('frontend'));

app.get('/price', (req, res) => {
    const symbol = req.query.symbol;
    if (!symbol) {
        return res.status(404).send('Not found');
    }
    yahooFinance.quote({
        symbol: symbol,
        modules: ['financialData']
    }, function (err, quotes) {
        if (quotes && quotes.financialData && quotes.financialData.currentPrice) {
            res.send({
                symbol: symbol,
                price: quotes.financialData.currentPrice
            });
        } else {
            return res.status(404).send('Not found');
        }
    });
})

app.listen(port, () => {
    console.log(`App listening at http://localhost:${port}`);
})