using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceApp.BankAdapters
{
    public sealed class GarantiBankAdapter : IBankAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public GarantiBankAdapter(HttpClient httpClient, string clientId, string clientSecret)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        }

        public async Task AuthenticateAsync(CancellationToken cancellationToken)
        {
            var payload = JsonConvert.SerializeObject(new { client_id = _clientId, client_secret = _clientSecret, grant_type = "client_credentials" });
            using (var response = await _httpClient.PostAsync("oauth/token", new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var token = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }
        }

        public async Task<IReadOnlyList<BankAccountDto>> GetAccountsAsync(CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync("api/accounts", cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<List<BankAccountDto>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        public async Task<IReadOnlyList<BankTransactionDto>> GetTransactionsAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync("api/transactions?start=" + start.ToString("yyyy-MM-dd") + "&end=" + end.ToString("yyyy-MM-dd"), cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<List<BankTransactionDto>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        public async Task<TransferResultDto> SendTransferAsync(TransferRequestDto request, CancellationToken cancellationToken)
        {
            var payload = JsonConvert.SerializeObject(request);
            using (var response = await _httpClient.PostAsync("api/transfers", new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<TransferResultDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        public async Task<TransferStatusDto> GetTransferStatusAsync(string referenceNo, CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync("api/transfers/" + referenceNo, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<TransferStatusDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        public async Task<IReadOnlyList<ExchangeRateDto>> GetExchangeRatesAsync(CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync("api/fx/rates", cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<List<ExchangeRateDto>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        private sealed class TokenResponse { [JsonProperty("access_token")] public string AccessToken { get; set; } }
    }
}
