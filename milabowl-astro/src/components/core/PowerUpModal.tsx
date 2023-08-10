export interface PowerUpModalProps {
    modal_id: string;  // THe ID of the modal to link button and modal
    name: string;  // The header name to display for this modal
    icon: string;  // The icon to display
    info: string;  // The information to display
}

/*
let icon = <svg className="mx-auto mb-4 text-gray-400 w-12 h-12 dark:text-gray-200" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10 11V6m0 8h.01M19 10a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                            </svg>
*/


const PowerUpModal: React.FC<PowerUpModalProps> = ({ modal_id, name, icon, info }) => {

    return (
        <div>
            <div id={modal_id} className="fixed top-0 left-0 right-0 z-50 hidden p-4 overflow-x-auto overflow-y-auto md:inset-0 h-[calc(100%-1rem)] max-h-full">
                <div className="relative w-full max-w-md max-h-full">
                    <div className="relative bg-white rounded-lg shadow dark:bg-gray-700">
                        <button type="button" className="absolute top-3 right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ml-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white" data-modal-hide={modal_id}>
                            <svg className="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                                <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                            </svg>
                            <span className="sr-only">Close modal</span>
                        </button>
                        <div className="p-6 text-center">
                            <h3 className="text-lg font-normal text-white ">{icon + "  " + name}</h3>
                            <div className="mb-5 font-normal text-gray-500 dark:text-gray-400">{info}</div>
                            <button data-modal-hide={modal_id} type="button"
                                className="text-white bg-red-600 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300
                                    dark:focus:ring-red-800 font-medium rounded-lg text-sm inline-flex items-center px-5 py-2.5 text-center mr-2">
                                Say what??
                            </button>
                            <button data-modal-hide={modal_id} type="button"
                                className="text-white bg-green-600 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300
                                rounded-lg text-sm font-medium px-5 py-2.5 hover:text-green-900 focus:z-10 dark:bg-green-700
                                dark:hover:text-white dark:hover:bg-green-800 dark:focus:ring-green-800">
                                Gotcha'!
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div >
    )
};

export default PowerUpModal;