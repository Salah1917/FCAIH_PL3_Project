using DictionaryUI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryUI.Controllers
{
    public class WordsController : Controller
    {
        #region Configurations
        public string BaseUrl = "https://localhost:7224/api/Words";
        public static HttpClient http = new();
        #endregion

        #region Index
        public async Task<IActionResult> Index(string search)
        {
            var allWords = await http.GetFromJsonAsync<List<WordsDTO>>(BaseUrl);

            if (string.IsNullOrWhiteSpace(search))
                return View(allWords);

            var searchResults = await http.GetFromJsonAsync<List<WordsDTO>>($"{BaseUrl}/search?query={search}");

            if (searchResults == null || !searchResults.Any())
                return View(allWords);

            return View(searchResults);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            var word = await http.GetFromJsonAsync<WordsDTO>($"{BaseUrl}/{id}");

            return View(word);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new WordsDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create(WordsDTO newWord)
        {
            newWord.Id = 0;
            if (ModelState.IsValid)
            {
                var res = await http.PostAsJsonAsync(BaseUrl, newWord);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Error creating word.");
                    return View(newWord);
                }
            }
            return View(newWord);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Word = await http.GetFromJsonAsync<WordsDTO>($"{BaseUrl}/{id}");
            if (Word is null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(Word);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(WordsDTO updatedWord)
        {
            var res = await http.PutAsJsonAsync($"{BaseUrl}/{updatedWord.Id}", updatedWord);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var res = await http.DeleteAsync($"{BaseUrl}/{id}");
            return RedirectToAction("Index");
        }
        #endregion
    }
}
