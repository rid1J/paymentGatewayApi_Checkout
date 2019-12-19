# Payment Gateway Api Checkout challenge 2.0

## Prerequisite: 
<ul> 
  <li>.NetCore 3.1</li>
  <li>
    Visual studio 2019
    <ul>
      <li>
        <h4>PaymentGatewayAPI</h4>
          Frameworks:
        <ol>
          <li>ASPNetCore</li>
          <li>NetCore App</li>
        </ol>
        Packages:
        <ol>
          <li>Jwt bearer</li>
          <li>core.Design</li>
          <li>Inmemory</li>
          <li>SqlServer</li>
          <li>CodeGeneration.Design</li>
        </ol>
      </li>
      <li>
        <h4>XUnit_PaymentGatewayAPITest</h4>
          Frameworks:
        <ol>
          <li>NetCore App</li>
        </ol>
        Packages:
        <ol>
          <li>Coverlet.Collector</li>
          <li>Net.test.sdk</li>
          <li>Moq</li>
          <li>xUnit</li>
          <li>xUnit.runner.visualstudio</li>
        </ol>
      </li>
    </ul>
  </li>
  <li>Postman v7.13.0</li>
  <li>Git for windows.</li>
  <li>Githud extension for visual studio 2019.</li>
  </ul>

## How to unpack:
<ol>
  <li>Click clone or download button.</li>
  <li>Click open in visual studio.</li>
  <li>In visual studio, team explorer tab, click clone and wait for project to clone and unpack locally.</li>
</ol>

## How to test: 
<ol>
  <li>Clone repository.</li>
  <li>Open paymentGatewayApi.sln in Visual studio 2019.</li>
  <li>
    Launch with IIS server.
    <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/1.PNG"/>
  </li>
  <li>Open Postman.</li>
  <li>Enter https://dev-viq3j6y4.auth0.com/oauth/token with method POST</li>
  <li>Paste following JSON in body: 
    {    
      "client_id": "OItuYdFhyNp6tTlhA0xugIcVDaCMussu",
      "client_secret": "NZn2WXegH5b3PI3wP4hll7j-lFFpDVo6NvHhqZqufumShjM5_-6FGLBH4B7akrjc",
      "audience": "paymentAuth",
      "grant_type": "client_credentials"
    }
  <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/3.PNG"/>
  </li>
  <li>
    Set header to Application-type/JSON
  <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/5.PNG"/>
  </li>
  <li>Send request.</li>
  <li>
    Copy access token from response and paste in Token with type Bearer token from tab Auth.
    <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/4.PNG"/>
  </li>
  <li>
    Copy and paste URL from browser to Postman.
    <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/2.PNG"/>
  </li>
  <li>Copy following JSON in body (Sample data):
    {
      "paymentMethod": "credit-card",
      "cardNumber": "4567042172880411",
      "cardType": "visa",
      "expDate": "12/2023",
      "cvv": "766",
      "merchantIdentifier": "appleInc12345",
      "totalAmount": 200,
      "currency": "EUR"
    }</li>
  <li>
    On clicking send, respond should return with status 200 OK and "Payment successful" message appears.
    <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/6.PNG"/>
  </li>
  <li>Copy merchantIdentifier from JSON above and append to link in postman.</li>
  <li>Change method from POST to GET and click send.</li>
  <li>
    Reponse should return with code 200 OK and list of transaction saved for this merchant Id.
    <img src="https://github.com/rid1J/paymentGatewayApi_Checkout/blob/master/scrshots/7.PNG"/>
  </li>
</ol>
