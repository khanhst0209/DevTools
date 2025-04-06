import { useState, useEffect } from "react";
import { Copy } from "lucide-react";
import { proxyApiCall } from "./utils/proxyApiCall";
import { requestClipboardCopy } from "./utils/requestClipboardCopy";

const Tool: React.FC = () => {
  const [word, setWord] = useState("");
  const [Hash_text, setHash_text] = useState("");

  useEffect(() => {
    const handleGenerateNumeronym = async () => {
      const data = await proxyApiCall<{ Hash_text: string }>("", "POST", {
        word: word,
      });

      console.log("Result from host:", data);
      setHash_text(data.Hash_text);
    };

    if (word.trim()) {
      handleGenerateNumeronym();
    } else {
      setHash_text("");
    }
  }, [word]);

  return (
    <div className="min-h-screen bg-white text-black flex flex-col items-center justify-center px-4 py-12 space-y-10 font-sans">
      <h1 className="text-4xl font-extrabold text-black tracking-tight">
        🔡 Numeronym Generator
      </h1>

      <input
        type="text"
        placeholder="Enter a word, e.g. 'internationalization'"
        value={word}
        onChange={(e) => setWord(e.target.value)}
        className="w-full max-w-xl border-4 border-black bg-gray-50 text-black px-6 py-5 rounded-2xl text-lg placeholder-gray-500 focus:outline-none focus:ring-4 focus:ring-black transition-all shadow-md"
      />

      <div className="text-4xl animate-bounce">⬇️</div>

      <div className="w-full max-w-xl relative">
        <input
          type="text"
          value={Hash_text || ""}
          placeholder="Your numeronym will be here, e.g. 'i18n'"
          readOnly
          className="w-full border-4 border-black bg-gray-100 text-black px-6 py-5 rounded-2xl text-lg placeholder-gray-500 focus:outline-none shadow-md"
        />
        <button
          onClick={() => requestClipboardCopy(Hash_text)}
          className="absolute right-5 top-1/2 -translate-y-1/2 text-gray-600 hover:text-black transition-colors"
          disabled={!Hash_text}
        >
          <Copy size={26} />
        </button>
      </div>
    </div>
  );
};

export default Tool;
