import { createTheme } from "@mui/material";

const theme = createTheme({
  palette: {
    primary: {
      main: "rgb(248,250,251)"
    },
    text: {
      primary: "#394473"
    }
  },
  typography: {
    fontFamily: ["Kumbh Sans", "sans-serif"].join(","),
    h5: {
      fontWeight: 700
    }
  },
  shadows: [
    "none",
    "rgba(149, 157, 165, 0.15) 0px 8px 24px;",
    "0px 2px 5px 0px rgba(0, 0, 0, 0.1),0px 2px 2px 0px rgba(0, 0, 0, 0.03),0px 3px 1px -2px rgba(0, 0, 0, 0.02)",
    "0px 2px 9px 0px rgba(0, 0, 0, 0.3),0px 1px 3px 0px rgba(0, 0, 0, 0.06),0px 3px 3px -2px rgba(0, 0, 0, 0.04)",
    "0px 4px 4px -1px rgba(0, 0, 0, 0.3),0px 0px 5px 0px rgba(0, 0, 0, 0.06),0px 1px 10px 0px rgba(0, 0, 0, 0.04)",
    "0px 6px 6px -1px rgba(0, 0, 0, 0.3),0px -1px 10px 0px rgba(0, 0, 0, 0.06),0px 1px 14px 0px rgba(0, 0, 0, 0.04)",
    "0px 6px 6px -1px rgba(0, 0, 0, 0.3),0px -2px 12px 0px rgba(0, 0, 0, 0.06),0px 1px 18px 0px rgba(0, 0, 0, 0.04)",
    "0px 7px 6px -2px rgba(0, 0, 0, 0.3),0px -1px 12px 1px rgba(0, 0, 0, 0.06),0px 2px 16px 1px rgba(0, 0, 0, 0.04)",
    "0px 10px 6px -3px rgba(0, 0, 0, 0.3),0px 0px 12px 1px rgba(0, 0, 0, 0.06),0px 3px 14px 2px rgba(0, 0, 0, 0.04)",
    "0px 10px 7px -3px rgba(0, 0, 0, 0.3),0px 1px 14px 1px rgba(0, 0, 0, 0.06),0px 3px 16px 2px rgba(0, 0, 0, 0.04)",
    "0px 11px 7px -3px rgba(0, 0, 0, 0.3),0px 2px 16px 1px rgba(0, 0, 0, 0.06),0px 4px 18px 3px rgba(0, 0, 0, 0.04)",
    "0px 11px 8px -4px rgba(0, 0, 0, 0.3),0px 3px 17px 1px rgba(0, 0, 0, 0.06),0px 4px 20px 3px rgba(0, 0, 0, 0.04)",
    "0px 13px 9px -4px rgba(0, 0, 0, 0.3),0px 4px 19px 2px rgba(0, 0, 0, 0.06),0px 5px 22px 4px rgba(0, 0, 0, 0.04)",
    "0px 13px 9px -4px rgba(0, 0, 0, 0.3),0px 5px 21px 2px rgba(0, 0, 0, 0.06),0px 5px 24px 4px rgba(0, 0, 0, 0.04)",
    "0px 13px 10px -4px rgba(0, 0, 0, 0.3),0px 6px 23px 2px rgba(0, 0, 0, 0.06),0px 5px 26px 4px rgba(0, 0, 0, 0.04)",
    "0px 15px 10px -5px rgba(0, 0, 0, 0.3),0px 7px 24px 2px rgba(0, 0, 0, 0.06),0px 6px 28px 5px rgba(0, 0, 0, 0.04)",
    "0px 15px 12px -5px rgba(0, 0, 0, 0.3),0px 8px 26px 2px rgba(0, 0, 0, 0.06),0px 6px 30px 5px rgba(0, 0, 0, 0.04)",
    "0px 15px 13px -5px rgba(0, 0, 0, 0.3),0px 9px 28px 2px rgba(0, 0, 0, 0.06),0px 6px 32px 5px rgba(0, 0, 0, 0.04)",
    "0px 17px 13px -5px rgba(0, 0, 0, 0.3),0px 10px 30px 2px rgba(0, 0, 0, 0.06),0px 7px 34px 6px rgba(0, 0, 0, 0.04)",
    "0px 17px 14px -6px rgba(0, 0, 0, 0.3),0px 11px 31px 2px rgba(0, 0, 0, 0.06),0px 7px 36px 6px rgba(0, 0, 0, 0.04)",
    "0px 19px 15px -6px rgba(0, 0, 0, 0.3),0px 12px 33px 3px rgba(0, 0, 0, 0.06),0px 8px 38px 7px rgba(0, 0, 0, 0.04)",
    "0px 19px 15px -6px rgba(0, 0, 0, 0.3),0px 13px 35px 3px rgba(0, 0, 0, 0.06),0px 8px 40px 7px rgba(0, 0, 0, 0.04)",
    "0px 19px 16px -6px rgba(0, 0, 0, 0.3),0px 14px 37px 3px rgba(0, 0, 0, 0.06),0px 8px 42px 7px rgba(0, 0, 0, 0.04)",
    "0px 20px 16px -7px rgba(0, 0, 0, 0.3),0px 15px 38px 3px rgba(0, 0, 0, 0.06),0px 9px 44px 8px rgba(0, 0, 0, 0.04)",
    "0px 20px 18px -7px rgba(0, 0, 0, 0.3),0px 16px 40px 3px rgba(0, 0, 0, 0.06),0px 9px 46px 8px rgba(0, 0, 0, 0.04)"
  ],
  components: {
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: "6px"
        }
      }
    },
    MuiTableCell: {
      styleOverrides: {
        head: {
          fontWeight: "bold"
        }
      }
    },
    MuiLink: {
      styleOverrides: {
        root: {
          color: "#394473",
          textDecoration: "underline"
        }
      }
    },
    MuiSlider: {
      styleOverrides: {
        rail: { color: "rgba(57,68,115,0.3)" },
        track: { color: "rgba(57,68,115,0.7)" }
      }
    }
  }
});

export default theme;
