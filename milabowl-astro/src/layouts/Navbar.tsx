import { useState } from "react";

interface NavbarProps {
  path: string;
}

const navLinks: { href: string, name: string }[] = [
	{
		href: "/blog",
		name: "Blog"
	},
	{
		href: "/nominations",
		name: "Nominations"
	},
	{
		href: "/standings",
		name: "Standings"
	},
];

const Navbar: React.FC<NavbarProps> = ({path}) => {
  const [menuOpen, toggleMenuOpen] = useState(false);
  return(
  <div className="min-h-full mb-4">
    <nav className="bg-indigo-800">
      <div className="max-w-screen-2xl mx-auto px-4 2xl:px-0">
        <div className="flex h-14 items-center justify-between">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              {/* <img className="h-8 w-8" src="./favicon.ico" alt="Your Company"/> */}
              <a href="/" className="text-white text-xl font-bold">
                Milabowl
              </a>              
            </div>
            <div className="hidden md:block">
              <div className="ml-10 flex items-baseline space-x-4">

                {
                  navLinks.map(m => {
                    const isSelected = path.includes(m.href);
                    const classes = isSelected ? "bg-indigo-900 text-white px-3 py-2 rounded-md text-sm font-medium" : 
                      "text-indigo-50 hover:bg-indigo-700 hover:text-white px-3 py-2 rounded-md text-sm font-medium";
                    return(
                    <a key={m.name} href={m.href} className={classes}>{m.name}</a>
                  )})
                }
              </div>
            </div>
          </div>
        
          <div className="-mr-2 flex md:hidden">
            <button 
              onClick={() => toggleMenuOpen(!menuOpen)}
              type="button" 
              className="inline-flex items-center justify-center rounded-md bg-indigo-800 p-2 text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-indigo-800" 
              aria-controls="mobile-menu" 
              aria-expanded="false"
            >
              <span className="sr-only">Open main menu</span>
              <svg className="block h-6 w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" aria-hidden="true">
                <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" />
              </svg>
              <svg className="hidden h-6 w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" aria-hidden="true">
                <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>
      </div>
      {
        menuOpen && (
          <div className="md:hidden" id="mobile-menu">
            <div className="space-y-1 px-2 pt-2 pb-3 sm:px-3">
              {
                navLinks.map(m => {
                  const isSelected = path.includes(m.href);
                  const classes = isSelected ? "bg-indigo-900 text-white block px-3 py-2 rounded-md text-base font-medium" : 
                    "text-gray-200 hover:bg-indigo-700 hover:text-white block px-3 py-2 rounded-md text-base font-medium";
                  return(
                  <a key={m.name} href={m.href} className={classes}>{m.name}</a>
                )})
              }
            </div>
          </div>
        )
      }
    </nav>
  </div>
)};

export default Navbar;