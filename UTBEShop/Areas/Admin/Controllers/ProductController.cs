using Microsoft.AspNetCore.Mvc;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Infrastructure.Database;
using UTBEShop.Areas.Admin.Models;
using TBU.eshop.web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using UTBEShop.Models.Entities.Identity;

namespace UTBEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //only manager and admin can actually access the following conrollers 
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class ProductController : Controller
    {
        readonly EShopDbContext _eshopDbContext;
        IWebHostEnvironment _webHostEnvironment;

        public ProductController(EShopDbContext eshopDbContext , IWebHostEnvironment webHostEnvironment)
        {
            _eshopDbContext = eshopDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        ProductViewModel productViewModel = new ProductViewModel();
        //1- Action to return the view of products that contains the table of products 
        public IActionResult Select()
        {
            //send our data to the view using the Product View Model 

            productViewModel.products = _eshopDbContext.Products.ToList();
            return View(productViewModel);
        }



        #region Action to delete an Item + Return Delete View
     
        public IActionResult Delete(int ID)
        {
            Product pro = _eshopDbContext.Products.FirstOrDefault(product => product.Id == ID);
            if(pro !=null)
            {
                _eshopDbContext.Products.Remove(pro);
                _eshopDbContext.SaveChanges();
                return RedirectToAction(nameof(Select));
            }
            return NotFound();
        }
            
        #endregion

        #region Action to Create an Item + Return Create View

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            //creating new instance of fileupload each new uploaded image 
            FileUpload fileUpload = new FileUpload(_webHostEnvironment.WebRootPath, "img/Products", "image");
            //uploading the image
            product.ProductSourceImage = await fileUpload.FileUploadAsync(product.Image);
            //clear the model from previous valdiation 
            ModelState.Clear();
            TryValidateModel(product);

            //-Server side validation
            if (ModelState.IsValid)
            {
                _eshopDbContext.Products.Add(product);
                _eshopDbContext.SaveChanges();
                return RedirectToAction(nameof(Select));
            }
            else
            {
                return View(product);
            }
        }


        #endregion

        #region Action to Edit an Item + Return Edit View 

        public IActionResult Edit(int id)
        {
            //1- Return to the view the product with ID sent from select page 
            Product product = _eshopDbContext.Products.FirstOrDefault(product => product.Id == id);

            return View(product);
        }
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(Product _product)
        {
            //-- reset the image source (that is submitted by user )
            _product.ProductSourceImage = "-";
            if(_product.Image != null)
            {
                FileUpload fileUpload = new FileUpload(_webHostEnvironment.WebRootPath, "img/Products", "image");
                _product.ProductSourceImage = await fileUpload.FileUploadAsync(_product.Image);
            }

            ModelState.Clear();
            TryValidateModel(_product);
            //-- Server Side Validation
            if (ModelState.IsValid)
            {
                _eshopDbContext.Entry(_eshopDbContext.Products.FirstOrDefault(product => product.Id == _product.Id)).CurrentValues.SetValues(_product);
                _eshopDbContext.SaveChanges();
                return RedirectToAction(nameof(Select));
            }
            else
            {
                return View(_product);
            }
        }

        #endregion

    }
}
