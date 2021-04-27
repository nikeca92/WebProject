//This is used on the Home screen to open a specific link
function HomePageItemClick(url)
{
   if(url == "")
      return;

   JSRedirect(url);
}