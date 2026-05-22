
using Microsoft.Maui.Controls;

namespace LandersSuperstore.Pages
{
    public partial class HomePage : ContentPage
    {
        private int _cartCount = 3;

    
        public HomePage()
        {
            InitializeComponent();
            LoadUserData();
        }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshCartBadge();
        }

        // ──────────────────────────────────────────────────────
        //  DATA LOADING
        // ──────────────────────────────────────────────────────
        private void LoadUserData()
        {
            // In a real app, load from ViewModel / API
            // ViewModel.LoadFeaturedProductsAsync();
            // ViewModel.LoadBestSellersAsync();
            // ViewModel.LoadRecentOrdersAsync();
        }

        private void RefreshCartBadge()
        {
            // Update cart count from service
            // _cartCount = CartService.Instance.Count;
        }

        // ──────────────────────────────────────────────────────
        //  HEADER BUTTON EVENTS
        // ──────────────────────────────────────────────────────
        private async void OnNotificationsClicked(object sender, EventArgs e)
        {
            // Navigate to Notifications Page
            await Navigation.PushAsync(new NotificationPage());
        }

       
        private async void OnCartClicked(object sender, EventArgs e)
        {
            // Navigate to Cart Page
            await Navigation.PushAsync(new CartPage());
        }


        // ─────────────────────────────────────────────────────
        //  SEARCH
        // ──────────────────────────────────────────────────────
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string query = e.NewTextValue?.Trim() ?? string.Empty;
            if (query.Length >= 2)
            {
                // Trigger live search suggestions
                // ViewModel.SearchProducts(query);
            }
        }

        private async void OnFilterClicked(object sender, EventArgs e)
        {
            // Show Filter Bottom Sheet
            await DisplayAlert("Filter", "Open filter options", "OK");
        }

        // ──────────────────────────────────────────────────────
        //  CATEGORIES
        // ──────────────────────────────────────────────────────
        private async void OnViewAllCategoriesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoriesPage());
        }

        private async void OnCategoryClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string category = btn.Text.Replace("🛒 ", "")
                                         .Replace("❄️ ", "")
                                         .Replace("🥤 ", "")
                                         .Replace("🥦 ", "")
                                         .Replace("🥩 ", "")
                                         .Replace("🌍 ", "")
                                         .Replace("🍞 ", "")
                                         .Replace("🥛 ", "")
                                         .Trim();

                await Shell.Current.GoToAsync($"ProductsPage?category={Uri.EscapeDataString(category)}");
            }
        }

        // ──────────────────────────────────────────────────────
        //  PRODUCTS
        // ──────────────────────────────────────────────────────
        private async void OnViewAllProductsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductsPage());
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            _cartCount++;

            // Haptic feedback
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);

            // Show confirmation toast / snackbar
            await DisplayAlert("Added to Cart! 🛒",
                               $"Item added. You now have {_cartCount} items in your cart.",
                               "OK");
        }

        private async void OnLoadMoreClicked(object sender, EventArgs e)
        {
            // Load next page of products
            // await ViewModel.LoadMoreProductsAsync();
            await DisplayAlert("Loading", "Fetching more products…", "OK");
        }

        // ──────────────────────────────────────────────────────
        //  ORDERS
        // ──────────────────────────────────────────────────────
        private async void OnViewAllOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());
        }

        // ──────────────────────────────────────────────────────
        //  BOTTOM NAVIGATION
        // ──────────────────────────────────────────────────────
        private async void OnNavCategoriesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoriesPage());
        }

        private async void OnNavWishlistClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WishlistPage());
        }

        private async void OnNavProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }
    }
}